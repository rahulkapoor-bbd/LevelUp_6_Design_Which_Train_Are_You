import axios, { AxiosError } from "axios";
import { configDotenv } from "dotenv";
import https from "https";

configDotenv();

const URL = process.env.API_URL;

const httpsAgent = new https.Agent({
  rejectUnauthorized: false, // Accept self-signed certificates
});

const axiosInstance = axios.create({
  httpsAgent,
});

let totals = [0, 0, 0, 0, 0];
let weights = [0, 0, 0, 0, 0];

function makeChoice(weight: number, trainId: number, isPositive: boolean) {
  totals[trainId - 1]++;
  weights[trainId - 1] += weight * (isPositive ? 1 : -1);
}

function submitResult(username: string): void {
  const percentages = weights.map((weight, index) =>
    totals[index] === 0 ? 0 : (weight / totals[index]) * 2
  );

  const max = Math.max(...percentages);
  const result = percentages.indexOf(max) + 1;

  console.log(`Update trainId to ${result} for ${username}`);

  axiosInstance.put(URL + "AppUser/updateTrainId", {
    username: username,
    trainId: result,
  });
}
