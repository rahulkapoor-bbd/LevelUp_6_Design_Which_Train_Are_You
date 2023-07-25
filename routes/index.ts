import express from "express";
import axios, { AxiosError } from "axios";
import https from "https";
const router = express.Router();

/* GET home page. */
router.get("/", function (req, res, next) {
  res.render("index", { title: "Which train are you?" });
});

router.get("/login", function (req, res, next) {
  res.render("login", { title: "Login" });
});

const httpsAgent = new https.Agent({
  rejectUnauthorized: false, // Accept self-signed certificates
});

const axiosInstance = axios.create({
  httpsAgent,
});

const QUIZ_URL = "https://localhost:7163/api/Question/quiz";

router.get("/quiz", async (req, res, next) => {
  try {
    const response = await axiosInstance.get<Question>(QUIZ_URL);
    const questions = response.data;
    res.render("quiz", {
      title: "Which train are you?",
      questions: questions,
    });
  } catch (error) {
    console.error("Error fetching questions:", (error as AxiosError<Error>).message);
    res.render("error");
  }
});

interface Question {
  question: string;
  id: number;
}

interface Error {
  message: string;
}

export default router;
