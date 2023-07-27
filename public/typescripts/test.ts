let totals = [0, 0, 0, 0, 0];
let weights = [0, 0, 0, 0, 0];

function makeChoice(weight: number, trainId: number, isPositive: boolean) {
  totals[trainId - 1]++;
  weights[trainId - 1] += weight * (isPositive ? 1 : -1);
}

function getResult(total: number): void {
  const percentages = weights.map(
    (weight, index) => weight / (totals[index] * 2)
  );

  const max = Math.max(...percentages);
  const result = percentages.indexOf(max) + 1;

  console.log(result);
}