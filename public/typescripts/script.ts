const submitButton = document.getElementById("quiz-submit");
const baseUrl = window.location.href.substring(
  0,
  window.location.href.indexOf("/")
);
let total = 0;
let answered = new Set();
let totals = [0, 0, 0, 0, 0];
let weights = [0, 0, 0, 0, 0];

function makeChoice(
  questionId: number,
  weight: number,
  trainId: number,
  isPositive: boolean
) {
  answered.add(questionId);
  totals[trainId - 1]++;
  weights[trainId - 1] += weight * (isPositive ? 1 : -1);
}

function prepareResult() {
  console.log("preparing result");
  const percentages = weights.map((weight, index) =>
    totals[index] === 0 ? 0 : (weight / totals[index]) * 2
  );

  const max = Math.max(...percentages);
  const result = percentages.indexOf(max) + 1;

  console.log(answered.size);
  if (answered.size === total) {
    localStorage.setItem("result", result + "");
  }
}

async function submitResult() {
  console.log("submitting result");
  const result = localStorage.getItem("result");

  await fetch(baseUrl + `submit/${result}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
  });

  window.location.replace(baseUrl + "profile");
}

function setTotal(totalQuestions: number) {
  total = totalQuestions;
}

if (submitButton) {
  submitButton.addEventListener("click", (event) => {
    console.log("submit clicked");
    prepareResult();
  });
}

if (localStorage.getItem("result")) {
  submitResult();
  localStorage.removeItem("result");
}
