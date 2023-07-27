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
      username: "Coolboi",
      questions: questions,
    });
  } catch (error) {
    console.error(
      "Error fetching questions:",
      (error as AxiosError<Error>).message
    );
    res.render("error");
  }
});

interface Question {
  id: number;
  content: string;
  trainId: number;
  isPositive: boolean;
}

interface Error {
  message: string;
}

router.get("/profile", async (req, res, next) => {
  res.render("profile", {
    title: "Profile Page",
    userDetails: {
      username: "CoolBoi",
      trainId: 1,
      trainName: "Union Pacific 9000 Class",
      description:
        "The Union Pacific Railroad 9000 Class was a class of 88 steam locomotives, built by ALCO for the Union Pacific between 1926 and 1930. The Union Pacific 9000 class was the only class of steam locomotives with a 4-12-2 wheel arrangement ever to be built.",
    },
  });
});

export default router;
