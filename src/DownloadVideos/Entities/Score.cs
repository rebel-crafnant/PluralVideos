using System.Collections.Generic;
using System.Linq;

namespace DownloadVideos.Entities
{
    public class Score
    {
        public int Id { get; set; }

        public int Success { get; set; }

        public int Failed { get; set; }
    }

    public static class ScoreExtensions
    {
        public static void Succeeded(this List<Score> scores, int index)
        {
            foreach (var score in scores)
            {
                if (score.Id == scores[index].Id)
                {
                    score.Success++;
                    break;
                }
            }

            scores.Reorder();
        }

        public static void Failed(this List<Score> scores, int index)
        {
            foreach (var score in scores)
            {
                if (score.Id == scores[index].Id)
                {
                    score.Failed++;
                    break;
                }
            }

            scores.Reorder();
        }

        private static void Reorder(this List<Score> scores)
        {
            scores = scores.OrderByDescending(x => x.Success)
                        .ThenBy(x => x.Failed).ToList();
        }
    }
}
