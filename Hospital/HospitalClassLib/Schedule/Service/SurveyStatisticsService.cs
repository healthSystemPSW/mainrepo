using Hospital.Schedule.Model.Wrappers;
using Hospital.Schedule.Repository;
using Hospital.SharedModel.Repository.Base;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Schedule.Service
{
    public class SurveyStatisticsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IAnsweredQuestionReadRepository _answeredQuestionRepo;
        public SurveyStatisticsService(IUnitOfWork uow)
        {
            _uow = uow;
            _answeredQuestionRepo = _uow.GetRepository<IAnsweredQuestionReadRepository>();

        }
        /**
         *  double AverageRating 
            int QuestionId 
            List<double> RatingCounts 
         */
        public List<QuestionStatistic> GetAverageQuestionRatingForAllSurveyQuestions()
        {
          
            var questionStatistics = _answeredQuestionRepo.GetAverageQuestionRatingForAllQuestions();
            /**
            double AverageRating 
            int QuestionId 
             */
            var ratingCounts = _answeredQuestionRepo.GetNumberOfEachRatingForEachQuestion();
            /**
             *      QuestionId 
                    Rating 
                    Count  
             */

            foreach (var statistic in questionStatistics)
            {
                statistic.RatingCounts = RatingCountsForOneQuestion(ratingCounts, statistic.QuestionId);
            }

            return questionStatistics;

        }
        public List<CategoryStatistic> GetAverageQuestionRatingForAllSurveyCategories()
        {
            var all = _answeredQuestionRepo.GetAverageQuestionRatingForAllCategories();
            return all;

        }
        /*
         Exposing private method state so it can be tested
         Ova metoda se poziva samo iz metoda servisa 
         */
        public static List<double> RatingCountsForOneQuestion(List<RatingCount> ratingCounts, int QuestionId)
        {
            double[] ratings = new double[5];
            foreach (var r in ratingCounts.Where(r => r.QuestionId.Equals(QuestionId)))
            {
                ratings[r.Rating - 1] = r.Count;
            }
            return ratings.ToList();

        }
    }
}
