﻿using HospitalUnitTests.Base;
using System;
using System.Linq;
using Hospital.Schedule.Model;
using Hospital.Schedule.Repository;
using Hospital.Schedule.Service;
using Hospital.SharedModel.Model.Enumerations;
using Shouldly;
using Xunit;

namespace HospitalUnitTests
{
    public class SurveyStatisticsTests : BaseTest
    {
        private readonly SurveyStatisticsService _surveyStatisticsService;
        public SurveyStatisticsTests(BaseFixture fixture) : base(fixture)
        {
            _surveyStatisticsService = new(UoW);
        }
        [Fact]
        public void Correct_average_rating_for_questions()
        {
            #region Arrange

            ClearDbContext();
            var survey = new Survey(true);
            Context.Surveys.Add(survey);
            Context.Questions.Add(new Question()
            {
                Id = 1,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 1"
            });
            Context.Questions.Add(new Question()
            {
                Id = 2,
                SurveyId = 1,
                Category = SurveyCategory.StaffSurvey,
                Text = "Pitanje 2"
            });
            Context.Questions.Add(new Question()
            {
                Id = 3,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 3"
            });

            var aQuestion1 = new AnsweredQuestion()
            {
                Id = 1,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 1,
                Rating = 2
            };
            var aQuestion2 = new AnsweredQuestion()
            {
                Id = 2,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 2,
                Rating = 4
            };
            var aQuestion3 = new AnsweredQuestion()
            {
                Id = 3,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 1,
                Rating = 3
            };
            var aQuestion4 = new AnsweredQuestion()
            {
                Id = 4,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 2,
                Rating = 4
            };
            var aQuestion5 = new AnsweredQuestion()
            {
                Id = 5,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 1,
                Rating = 5
            };
            var aQuestion6 = new AnsweredQuestion()
            {
                Id = 6,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 2,
                Rating = 1
            };
            Context.AnsweredQuestions.Add(aQuestion1);
            Context.AnsweredQuestions.Add(aQuestion2);
            Context.AnsweredQuestions.Add(aQuestion3);
            Context.AnsweredQuestions.Add(aQuestion4);
            Context.AnsweredQuestions.Add(aQuestion5);
            Context.AnsweredQuestions.Add(aQuestion6);

            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 1).ToList(), new DateTime(), 1, survey, 1, null, 1, null));

            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 2).ToList(), new DateTime(), 1, survey, 1, null, 1, null));
            Context.SaveChanges();
            #endregion
            
            var temp = _surveyStatisticsService.GetAverageQuestionRatingForAllSurveyQuestions().OrderBy(o => o.QuestionId).ToList();
            double avg1 = temp[0].AverageRating;
            double avg2 = temp[1].AverageRating;
            avg1.ShouldBe(3);
            avg2.ShouldBe(3.5);

        }
        [Fact]
        
        public void Incorrect_average_rating_for_questions()
        {
            #region Arrange

            ClearDbContext();
            var survey = new Survey(true);
            Context.Surveys.Add(survey);
            Context.Questions.Add(new Question()
            {
                Id = 1,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 1"
            });
            Context.Questions.Add(new Question()
            {
                Id = 2,
                SurveyId = 1,
                Category = SurveyCategory.StaffSurvey,
                Text = "Pitanje 2"
            });
            Context.Questions.Add(new Question()
            {
                Id = 3,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 3"
            });
            Context.Questions.Add(new Question()
            {
                Id = 4,
                SurveyId = 1,
                Category = SurveyCategory.HospitalSurvey,
                Text = "Pitanje 4"
            });

            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 1,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 1,
                Rating = 2
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 2,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 2,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 3,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 1,
                Rating = 3
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 4,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 2,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 5,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 1,
                Rating = 5
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 6,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 2,
                Rating = 1
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 7,
                Category = SurveyCategory.HospitalSurvey,
                QuestionId = 4,
                AnsweredSurveyId = 1,
                Rating = 4
            });

            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 1).ToList(), new DateTime(), 1, survey, 1, null, 1, null));

            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 2).ToList(), new DateTime(), 1, survey, 1, null, 1, null));

            Context.SaveChanges();
            #endregion 
    
            double avg = _surveyStatisticsService.GetAverageQuestionRatingForAllSurveyQuestions()[2].AverageRating;
            avg.ShouldNotBe(2);
        }

        [Fact]
        public void Correct_average_rating_for_section()
        {
            #region Arrange

            ClearDbContext();
            var survey = new Survey(true);
            Context.Surveys.Add(survey);
            Context.Questions.Add(new Question()
            {
                Id = 1,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 1"
            });
            Context.Questions.Add(new Question()
            {
                Id = 2,
                SurveyId = 1,
                Category = SurveyCategory.StaffSurvey,
                Text = "Pitanje 2"
            });
            Context.Questions.Add(new Question()
            {
                Id = 3,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 3"
            });
            Context.Questions.Add(new Question()
            {
                Id = 4,
                SurveyId = 1,
                Category = SurveyCategory.HospitalSurvey,
                Text = "Pitanje 4"
            });

            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 1,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 1,
                Rating = 2
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 2,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 2,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 3,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 1,
                Rating = 3
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 4,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 2,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 5,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 1,
                Rating = 5
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 6,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 2,
                Rating = 1
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 7,
                Category = SurveyCategory.HospitalSurvey,
                QuestionId = 4,
                AnsweredSurveyId = 1,
                Rating = 4
            });
            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 1).ToList(), new DateTime(), 1, survey, 1, null, 1, null));

            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 2).ToList(), new DateTime(), 1, survey, 1, null, 1, null));

            Context.SaveChanges();
            #endregion

            var category = _surveyStatisticsService.GetAverageQuestionRatingForAllSurveyCategories()
                .Where(x => x.Category.Equals(SurveyCategory.DoctorSurvey)).ToList();
            var avg = category.First().AverageRating;
            avg.ShouldBe(3);
        }

        [Fact]
        public void Incorrect_average_rating_for_section()
        {
            #region Arrange

            ClearDbContext();
            var survey = new Survey(true);
            Context.Surveys.Add(survey);
            Context.Questions.Add(new Question()
            {
                Id = 1,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 1"
            });
            Context.Questions.Add(new Question()
            {
                Id = 2,
                SurveyId = 1,
                Category = SurveyCategory.StaffSurvey,
                Text = "Pitanje 2"
            });
            Context.Questions.Add(new Question()
            {
                Id = 3,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 3"
            });
            Context.Questions.Add(new Question()
            {
                Id = 4,
                SurveyId = 1,
                Category = SurveyCategory.HospitalSurvey,
                Text = "Pitanje 4"
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 1,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 1,
                Rating = 2
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 2,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 2,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 3,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 1,
                Rating = 3
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 4,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 2,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 5,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 1,
                Rating = 5
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 6,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 2,
                Rating = 1
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 7,
                Category = SurveyCategory.HospitalSurvey,
                QuestionId = 4,
                AnsweredSurveyId = 1,
                Rating = 4
            });

            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 1).ToList(), new DateTime(), 1, survey, 1, null, 1, null));

            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 2).ToList(), new DateTime(), 1, survey, 1, null, 1, null));

            Context.SaveChanges();
            #endregion
     
            var category = _surveyStatisticsService.GetAverageQuestionRatingForAllSurveyCategories()
                .Where(x => x.Category.Equals(SurveyCategory.HospitalSurvey)).ToList();
            var avg = category.First().AverageRating;
            avg.ShouldNotBe(3);
        }

        [Fact]
        public void Correct_number_of_each_rating()
        {
            #region Arrange

            ClearDbContext();
            var survey = new Survey(true);
            Context.Surveys.Add(survey);
            Context.Questions.Add(new Question()
            {
                Id = 1,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 1"
            });
            Context.Questions.Add(new Question()
            {
                Id = 2,
                SurveyId = 1,
                Category = SurveyCategory.StaffSurvey,
                Text = "Pitanje 2"
            });
            Context.Questions.Add(new Question()
            {
                Id = 3,
                SurveyId = 1,
                Category = SurveyCategory.DoctorSurvey,
                Text = "Pitanje 3"
            });
            Context.Questions.Add(new Question()
            {
                Id = 4,
                SurveyId = 1,
                Category = SurveyCategory.HospitalSurvey,
                Text = "Pitanje 4"
            });

            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 1,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 1,
                Rating = 2
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 2,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 2,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 3,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 1,
                Rating = 3
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 4,
                Category = SurveyCategory.StaffSurvey,
                QuestionId = 2,
                AnsweredSurveyId = 2,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 5,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 1,
                Rating = 5
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 6,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 3,
                AnsweredSurveyId = 2,
                Rating = 1
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 7,
                Category = SurveyCategory.HospitalSurvey,
                QuestionId = 4,
                AnsweredSurveyId = 1,
                Rating = 4
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 8,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 1,
                Rating = 2
            });
            Context.AnsweredQuestions.Add(new AnsweredQuestion()
            {
                Id = 9,
                Category = SurveyCategory.DoctorSurvey,
                QuestionId = 1,
                AnsweredSurveyId = 2,
                Rating = 2
            });
            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 1).ToList(), new DateTime(), 1, survey, 1, null, 1, null));

            Context.AnsweredSurveys.Add(new AnsweredSurvey(UoW.GetRepository<IAnsweredQuestionReadRepository>().GetAll().Where(x => x.AnsweredSurveyId == 2).ToList(), new DateTime(), 1, survey, 1, null, 1, null));
            Context.SaveChanges();
            #endregion

            var repo = UoW.GetRepository<IAnsweredQuestionReadRepository>();
          
            var ratings = repo.GetNumberOfEachRatingForEachQuestion();
            var countsForQuestion = SurveyStatisticsService.RatingCountsForOneQuestion(ratings, 1);
            countsForQuestion[0].ShouldBe(0);
            countsForQuestion[1].ShouldBe(3);
            countsForQuestion[2].ShouldBe(0);
            countsForQuestion[3].ShouldBe(1);
            countsForQuestion[4].ShouldBe(0);
        }
    }
}
