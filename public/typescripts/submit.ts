import { submitResult } from './script';
const submitButton = document.getElementById('quiz-submit');

if (submitButton) {
  submitButton.addEventListener('click', (event) => {
    event.preventDefault(); // Prevent the default form submission
    console.log("Submit clicked");
    submitResult();
  });
}
