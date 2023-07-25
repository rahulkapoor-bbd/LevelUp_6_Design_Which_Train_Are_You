var express = require("express");
var router = express.Router();

/* GET home page. */
router.get("/", function (req, res, next) {
  res.render("index", { title: "Which train are you?" });
});

router.get("/login", function (req, res, next) {
  res.render("login", { title: "Login" });
});

// TODO: Add in the api url
const API_URL = "";

// This is temp:
const questions = [
  {
    question: "I always have kind things to say to other people.",
    id: 1,
  },
  {
    question: "I am a good listener.",
    id: 2,
  },
  {
    question: "I often feel down or sad.",
    id: 3,
  },
];

router.get("/quiz", function (req, res, next) {
  // try {
  // const response = await axios.get(API_URL);
  // const questions = response.data;
  // res.render("quiz", { questions });
  // } catch (error) {
  // console.error("Error fetching questions:", error.message);
  // res.render("error");
  // }
  res.render("quiz", { title: "Which train are you?", questions: questions });
});

module.exports = router;
