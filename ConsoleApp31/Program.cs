using System;
using System.Collections.Generic;

namespace QuizApp
{
    // Класс, представляющий вопрос викторины
    public class Question
    {
        public string Text { get; set; }
        public List<string> Choices { get; set; }
        public List<int> CorrectChoices { get; set; }

        public Question(string text, List<string> choices, List<int> correctChoices)
        {
            Text = text;
            Choices = choices;
            CorrectChoices = correctChoices;
        }
    }

    // Класс, представляющий викторину
    public class Quiz
    {
        public string Name { get; set; }
        public List<Question> Questions { get; set; }

        public Quiz(string name, List<Question> questions)
        {
            Name = name;
            Questions = questions;
        }
    }

    // Класс, представляющий пользователя
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<QuizResult> QuizResults { get; set; }

        public User(string username, string password, DateTime dateOfBirth)
        {
            Username = username;
            Password = password;
            DateOfBirth = dateOfBirth;
            QuizResults = new List<QuizResult>();
        }
    }

    // Класс, представляющий результаты викторины пользователя
    public class QuizResult
    {
        public Quiz Quiz { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }

        public QuizResult(Quiz quiz, int correctAnswers, int totalQuestions)
        {
            Quiz = quiz;
            CorrectAnswers = correctAnswers;
            TotalQuestions = totalQuestions;
        }
    }

    // Класс, представляющий приложение Викторина
    public class QuizApplication
    {
        public List<User> users;
        public List<Quiz> quizzes;

        public QuizApplication()
        {
            users = new List<User>();
            quizzes = new List<Quiz>();
        }

        // Метод регистрации нового пользователя
        public void RegisterUser(string username, string password, DateTime dateOfBirth)
        {
            if (UserExists(username))
            {
                Console.WriteLine("Пользователь с таким логином уже существует.");
                return;
            }

            User newUser = new User(username, password, dateOfBirth);
            users.Add(newUser);
            Console.WriteLine("Регистрация успешно завершена.");
        }

        // Метод проверки существования пользователя
        public bool UserExists(string username)
        {
            foreach (User user in users)
            {
                if (user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        // Метод аутентификации пользователя
        public User AuthenticateUser(string username, string password)
        {
            foreach (User user in users)
            {
                if (user.Username == username && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        // Метод старта новой викторины для пользователя
        public void StartQuiz(User user, string quizName)
        {
            Quiz selectedQuiz = GetQuizByName(quizName);
            if (selectedQuiz == null)
            {
                Console.WriteLine("Викторина не найдена.");
                return;
            }

            List<Question> quizQuestions = selectedQuiz.Questions;
            int totalQuestions = quizQuestions.Count;
            int correctAnswers = 0;

            Console.WriteLine("Отвечайте на вопросы (укажите номера правильных ответов через запятую):");
            for (int i = 0; i < totalQuestions; i++)
            {
                Question question = quizQuestions[i];
                Console.WriteLine($"Вопрос {i + 1}: {question.Text}");

                for (int j = 0; j < question.Choices.Count; j++)
                {
                    Console.WriteLine($"{j + 1}. {question.Choices[j]}");
                }

                string answerInput = Console.ReadLine();
                string[] answerChoices = answerInput.Split(',');

                List<int> userChoices = new List<int>();
                foreach (string choice in answerChoices)
                {
                    if (int.TryParse(choice, out int choiceIndex))
                    {
                        userChoices.Add(choiceIndex - 1);
                    }
                }

                if (AreChoicesCorrect(question, userChoices))
                {
                    correctAnswers++;
                }
            }

            QuizResult quizResult = new QuizResult(selectedQuiz, correctAnswers, totalQuestions);
            user.QuizResults.Add(quizResult);

            Console.WriteLine($"Вы ответили правильно на {correctAnswers} из {totalQuestions} вопросов.");
        }

        // Метод проверки правильности ответов пользователя
        public bool AreChoicesCorrect(Question question, List<int> userChoices)
        {
            if (question.CorrectChoices.Count != userChoices.Count)
            {
                return false;
            }

            foreach (int choiceIndex in userChoices)
            {
                if (!question.CorrectChoices.Contains(choiceIndex))
                {
                    return false;
                }
            }

            return true;
        }

        // Метод получения викторины по имени
        public Quiz GetQuizByName(string quizName)
        {
            foreach (Quiz quiz in quizzes)
            {
                if (quiz.Name == quizName)
                {
                    return quiz;
                }
            }
            return null;
        }

        // Метод создания новой викторины
        public void CreateQuiz(string name)
        {
            if (QuizExists(name))
            {
                Console.WriteLine("Викторина с таким названием уже существует.");
                return;
            }

            List<Question> questions = new List<Question>();
            while (true)
            {
                Console.WriteLine("Создание вопроса (для выхода введите 'exit'):");

                Console.Write("Введите текст вопроса: ");
                string questionText = Console.ReadLine();

                if (questionText == "exit")
                {
                    break;
                }

                List<string> choices = new List<string>();
                while (true)
                {
                    Console.WriteLine("Введите вариант ответа (для выхода введите 'exit'):");
                    string choice = Console.ReadLine();

                    if (choice == "exit")
                    {
                        break;
                    }

                    choices.Add(choice);
                }

                Console.Write("Введите номера правильных ответов через запятую: ");
                string correctAnswerInput = Console.ReadLine();
                string[] correctAnswerChoices = correctAnswerInput.Split(',');

                List<int> correctChoices = new List<int>();
                foreach (string choice in correctAnswerChoices)
                {
                    if (int.TryParse(choice, out int choiceIndex))
                    {
                        correctChoices.Add(choiceIndex - 1);
                    }
                }

                Question question = new Question(questionText, choices, correctChoices);
                questions.Add(question);
            }

            Quiz newQuiz = new Quiz(name, questions);
            quizzes.Add(newQuiz);

            Console.WriteLine($"Викторина '{name}' успешно создана.");
        }

        // Метод редактирования викторины
        public void EditQuiz(string name)
        {
            Quiz quiz = GetQuizByName(name);
            if (quiz == null)
            {
                Console.WriteLine("Викторина не найдена.");
                return;
            }

            while (true)
            {
                Console.WriteLine($"Редактирование викторины '{name}' (для выхода введите 'exit'):");
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить вопрос");
                Console.WriteLine("2. Удалить вопрос");
                Console.WriteLine("3. Редактировать вопрос");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.WriteLine("Создание нового вопроса:");
                    Console.Write("Введите текст вопроса: ");
                    string questionText = Console.ReadLine();

                    List<string> choices = new List<string>();
                    while (true)
                    {
                        Console.WriteLine("Введите вариант ответа (для выхода введите 'exit'):");
                        string choice = Console.ReadLine();

                        if (choice == "exit")
                        {
                            break;
                        }

                        choices.Add(choice);
                    }

                    Console.Write("Введите номера правильных ответов через запятую: ");
                    string correctAnswerInput = Console.ReadLine();
                    string[] correctAnswerChoices = correctAnswerInput.Split(',');

                    List<int> correctChoices = new List<int>();
                    foreach (string choice in correctAnswerChoices)
                    {
                        if (int.TryParse(choice, out int choiceIndex))
                        {
                            correctChoices.Add(choiceIndex - 1);
                        }
                    }

                    Question question = new Question(questionText, choices, correctChoices);
                    quiz.Questions.Add(question);

                    Console.WriteLine("Вопрос успешно добавлен.");
                }
                else if (input == "2")
                {
                    Console.Write("Введите номер вопроса для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int questionNumber))
                    {
                        if (questionNumber >= 1 && questionNumber <= quiz.Questions.Count)
                        {
                            quiz.Questions.RemoveAt(questionNumber - 1);
                            Console.WriteLine("Вопрос успешно удален.");
                        }
                        else
                        {
                            Console.WriteLine("Неверный номер вопроса.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод.");
                    }
                }
                else if (input == "3")
                {
                    Console.Write("Введите номер вопроса для редактирования: ");
                    if (int.TryParse(Console.ReadLine(), out int questionNumber))
                    {
                        if (questionNumber >= 1 && questionNumber <= quiz.Questions.Count)
                        {
                            Question question = quiz.Questions[questionNumber - 1];

                            Console.WriteLine("Редактирование вопроса:");
                            Console.WriteLine($"Текущий текст вопроса: {question.Text}");
                            Console.Write("Введите новый текст вопроса: ");
                            string newQuestionText = Console.ReadLine();
                            question.Text = newQuestionText;

                            Console.WriteLine("Текущие варианты ответов:");
                            for (int i = 0; i < question.Choices.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {question.Choices[i]}");
                            }

                            Console.WriteLine("Редактирование вариантов ответов (для удаления варианта введите 'delete', для выхода введите 'exit'):");
                            for (int i = 0; i < question.Choices.Count; i++)
                            {
                                Console.Write($"Введите новый текст для варианта {i + 1}: ");
                                string choice = Console.ReadLine();

                                if (choice == "delete")
                                {
                                    question.Choices.RemoveAt(i);
                                    i--;
                                }
                                else if (choice == "exit")
                                {
                                    break;
                                }
                                else
                                {
                                    question.Choices[i] = choice;
                                }
                            }

                            Console.WriteLine("Текущие правильные ответы:");
                            for (int i = 0; i < question.CorrectChoices.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {question.Choices[question.CorrectChoices[i]]}");
                            }

                            Console.WriteLine("Редактирование правильных ответов (для удаления ответа введите 'delete', для выхода введите 'exit'):");
                            for (int i = 0; i < question.CorrectChoices.Count; i++)
                            {
                                Console.Write($"Введите новый номер правильного ответа {i + 1}: ");
                                if (int.TryParse(Console.ReadLine(), out int choiceIndex))
                                {
                                    if (choiceIndex >= 1 && choiceIndex <= question.Choices.Count)
                                    {
                                        question.CorrectChoices[i] = choiceIndex - 1;
                                    }
                                    else if (choiceIndex == 0)
                                    {
                                        Console.WriteLine("Номер варианта ответа должен быть положительным числом.");
                                        i--;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неверный номер варианта ответа.");
                                        i--;
                                    }
                                }
                                else if (Console.ReadLine() == "delete")
                                {
                                    question.CorrectChoices.RemoveAt(i);
                                    i--;
                                }
                                else if (Console.ReadLine() == "exit")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Неверный ввод.");
                                    i--;
                                }
                            }

                            Console.WriteLine("Вопрос успешно отредактирован.");
                        }
                        else
                        {
                            Console.WriteLine("Неверный номер вопроса.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный ввод.");
                    }
                }
                else if (input == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                }
            }
        }

        // Метод проверки существования викторины
        public bool QuizExists(string name)
        {
            foreach (Quiz quiz in quizzes)
            {
                if (quiz.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
    }

    // Главный класс программы
    public class Program
    {
        static void Main(string[] args)
        {
            QuizApplication quizApp = new QuizApplication();

            // Создание и добавление викторин
            List<Question> historyQuestions = new List<Question>
            {
                new Question("Вопрос 1", new List<string> { "Ответ 1", "Ответ 2", "Ответ 3" }, new List<int> { 1 }),
                new Question("Вопрос 2", new List<string> { "Ответ 1", "Ответ 2", "Ответ 3" }, new List<int> { 2 }),
                // ...
            };
            Quiz historyQuiz = new Quiz("История", historyQuestions);
            quizApp.quizzes.Add(historyQuiz);

            List<Question> geographyQuestions = new List<Question>
            {
                new Question("Вопрос 1", new List<string> { "Ответ 1", "Ответ 2", "Ответ 3" }, new List<int> { 1 }),
                new Question("Вопрос 2", new List<string> { "Ответ 1", "Ответ 2", "Ответ 3" }, new List<int> { 2 }),
                // ...
            };
            Quiz geographyQuiz = new Quiz("География", geographyQuestions);
            quizApp.quizzes.Add(geographyQuiz);

            // Пример использования приложения Викторина
            Console.WriteLine("Добро пожаловать в приложение Викторина!");

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Регистрация");
                Console.WriteLine("2. Вход");
                Console.WriteLine("3. Выход");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("Введите логин: ");
                    string username = Console.ReadLine();
                    Console.Write("Введите пароль: ");
                    string password = Console.ReadLine();
                    Console.Write("Введите дату рождения (гггг-мм-дд): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
                    {
                        quizApp.RegisterUser(username, password, dateOfBirth);
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат даты рождения.");
                    }
                }
                else if (input == "2")
                {
                    Console.Write("Введите логин: ");
                    string username = Console.ReadLine();
                    Console.Write("Введите пароль: ");
                    string password = Console.ReadLine();

                    User user = quizApp.AuthenticateUser(username, password);
                    if (user != null)
                    {
                        Console.WriteLine($"Добро пожаловать, {user.Username}!");

                        while (true)
                        {
                            Console.WriteLine("Выберите действие:");
                            Console.WriteLine("1. Стартовать новую викторину");
                            Console.WriteLine("2. Посмотреть результаты прошлых викторин");
                            Console.WriteLine("3. Посмотреть Топ-20 по конкретной викторине");
                            Console.WriteLine("4. Изменить настройки");
                            Console.WriteLine("5. Создать викторину");
                            Console.WriteLine("6. Редактировать викторину");
                            Console.WriteLine("7. Выход");

                            string userChoice = Console.ReadLine();

                            if (userChoice == "1")
                            {
                                Console.WriteLine("Выберите викторину:");
                                foreach (Quiz quiz in quizApp.quizzes)
                                {
                                    Console.WriteLine(quiz.Name);
                                }
                                string quizName = Console.ReadLine();

                                quizApp.StartQuiz(user, quizName);
                            }
                            else if (userChoice == "2")
                            {
                                Console.WriteLine("Результаты прошлых викторин:");
                                foreach (QuizResult result in user.QuizResults)
                                {
                                    Console.WriteLine($"{result.Quiz.Name}: {result.CorrectAnswers}/{result.TotalQuestions}");
                                }
                            }
                            else if (userChoice == "3")
                            {
                                Console.WriteLine("Введите название викторины:");
                                string quizName = Console.ReadLine();

                                Quiz selectedQuiz = quizApp.GetQuizByName(quizName);
                                if (selectedQuiz != null)
                                {
                                    Console.WriteLine($"Топ-20 по викторине {quizName}:");
                                    // Вывести Топ-20 результатов по выбранной викторине
                                }
                                else
                                {
                                    Console.WriteLine("Викторина не найдена.");
                                }
                            }
                            else if (userChoice == "4")
                            {
                                Console.WriteLine("Изменение настроек пользователя:");
                                Console.WriteLine("1. Изменить пароль");
                                Console.WriteLine("2. Изменить дату рождения");
                                string settingChoice = Console.ReadLine();

                                if (settingChoice == "1")
                                {
                                    Console.Write("Введите новый пароль: ");
                                    string newPassword = Console.ReadLine();
                                    user.Password = newPassword;
                                    Console.WriteLine("Пароль успешно изменен.");
                                }
                                else if (settingChoice == "2")
                                {
                                    Console.Write("Введите новую дату рождения (гггг-мм-дд): ");
                                    if (DateTime.TryParse(Console.ReadLine(), out DateTime newDateOfBirth))
                                    {
                                        user.DateOfBirth = newDateOfBirth;
                                        Console.WriteLine("Дата рождения успешно изменена.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неверный формат даты рождения.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Неверный выбор.");
                                }
                            }
                            else if (userChoice == "5")
                            {
                                Console.Write("Введите название новой викторины: ");
                                string newQuizName = Console.ReadLine();
                                quizApp.CreateQuiz(newQuizName);
                            }
                            else if (userChoice == "6")
                            {
                                Console.Write("Введите название викторины для редактирования: ");
                                string editQuizName = Console.ReadLine();
                                quizApp.EditQuiz(editQuizName);
                            }
                            else if (userChoice == "7")
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Неверный выбор.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный логин или пароль.");
                    }
                }
                else if (input == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                }
            }

            Console.WriteLine("Спасибо за использование приложения Викторина. До свидания!");
        }
    }
}
