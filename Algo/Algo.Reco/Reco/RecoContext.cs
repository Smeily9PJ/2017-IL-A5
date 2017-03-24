using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Algo
{
    public class RecoContext
    {
        public User[] Users { get; private set; }
        public Movie[] Movies { get; private set; }

        public void LoadFrom(string folder)
        {
            Users = User.ReadUsers(Path.Combine(folder, "users.dat"));
            Movies = Movie.ReadMovies(Path.Combine(folder, "movies.dat"));
            User.ReadRatings(Users, Movies, Path.Combine(folder, "ratings.dat"));
        }

        public double DistNorm2(User u1, User u2)
        {
            // prendre les deux rating, prendre celui qui en a vue le moins et boucler dessus.
           /* var commun = u1.Ratings.Intersect(u2.Ratings);

            var uMinRating = u1.Ratings.Count > u2.Ratings.Count ? u2.Ratings : u1.Ratings;
            var uMaxRating = u1.Ratings.Count > u2.Ratings.Count ? u1.Ratings : u2.Ratings;
            var communRating = new List<List<int>>();
            var diffs = new List<Double>();
            var carre = new List<Double>();
            foreach (var moviesMin in uMinRating)
            {
                if (uMaxRating.ContainsKey(moviesMin.Key))
                {
                    communRating.Add(new List<int> { moviesMin.Value, uMaxRating[moviesMin.Key] });
                    Double diff = Math.Abs(moviesMin.Value - uMaxRating[moviesMin.Key]);
                    Double exposant = 2;
                    diffs.Add(diff);
                    carre.Add(Math.Pow(diff, exposant));
                }
            }

            return Math.Sqrt(carre.Sum(x => x));*/

            var sum = u1.Ratings.Select(mr1 => new
                                    {
                                        R1 = mr1.Value,
                                        R2 = u2.Ratings.GetValueWithDefault(mr1.Key, -1)
                                    })
                                .Where(r1r2 => r1r2.R2 >= 0)
                                .Select(r1r2 => r1r2.R1 - r1r2.R2)
                                .Select(delta => delta * delta)
                                .Sum();

            return Math.Sqrt(sum);
        }
    }

    public static class DictionaryExtension
    {
        public static TValue GetValueWithDefault<TKey, TValue>( this Dictionary<TKey, TValue> @this, TKey key, TValue def)
        {
            TValue v;
            return @this.TryGetValue(key, out v) ? v : def;
        }
    }
}
