import express from "express";
import axios, { AxiosError } from "axios";
import https from "https";
const router = express.Router();
import { configDotenv } from "dotenv";

configDotenv();

const URL = process.env.API_URL;

const httpsAgent = new https.Agent({
  rejectUnauthorized: false, // Accept self-signed certificates
});

const axiosInstance = axios.create({
  httpsAgent,
});

/* GET home page. */
router.get("/", function (req, res, next) {
  res.render("index", { title: "Which train are you?" });
});

router.get("/login", function (req, res, next) {
  res.render("login", { title: "Login" });
});

router.post("/login", async (req, res, next) => {
  const loginUrl = URL + "AppUser/login";
  const loginData = JSON.stringify({
    username: req.body.username,
    password: req.body.password,
  });
  try {
    const response = await axiosInstance.post(loginUrl, loginData, {
      headers: { "Content-Type": "application/json" },
    });
    res.redirect("/quiz");
  } catch (error: any) {
    console.error(error);
    res.render("login", { title: "Login", error: error.response.data });
  }
});

router.get("/register", function (req, res, next) {
  res.render("register", { title: "Register" });
});

router.post("/register", async (req, res, next) => {
  const registerUrl = URL + "AppUser/register";
  const registerData = JSON.stringify({
    username: req.body.username,
    password: req.body.password,
  });
  try {
    const response = await axiosInstance.post(registerUrl, registerData, {
      headers: { "Content-Type": "application/json" },
    });
    console.log(response.status);
    res.redirect("/login");
  } catch (error) {
    console.error(error);
    res.render("register", { title: "Register", error: error });
  }
});

router.get("/quiz", async (req, res, next) => {
  const quizUrl = URL + "Question/quiz";
  try {
    const response = await axiosInstance.get<Question>(quizUrl);
    const questions = response.data;
    res.render("quiz", {
      title: "Which train are you?",
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

export default router;
