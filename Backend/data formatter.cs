using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Durhack
{
    struct Interval
    {
        public float min;
        public float max;
        public float delta;
        public float[] totals;
        public float[] gold;
        public float[] bronze;
        public float[] silver;
        public float[] none;

        public void AddMedal(float value, string medal)
        {
            int interval = (int)((value - min) / delta);
            totals[interval] += 1;
            switch (medal)
            {
                case "Gold": gold[interval] += 1; break;
                case "Silver": silver[interval] += 1; break;
                case "Bronze": bronze[interval] += 1; break;
                case "NA": none[interval] += 1; break;
            }
        }

        public float GetProbability(float value, string medal)
        {
            int interval = (int)((value - min) / delta);
            switch (medal)
            {
                case "Gold": return gold[interval];
                case "Silver": return silver[interval];
                case "Bronze": return bronze[interval];
                case "NA": return none[interval];
            }
            return 0;
        }

        public override string ToString()
        {
            string intervals = "";
            string seperator = "";
            for (int i = 0; i < gold.Length; i++)
            {
                intervals += seperator + gold[i];
                seperator = ",";
            }

            intervals += ",";

            seperator = "";
            for (int i = 0; i < silver.Length; i++)
            {
                intervals += seperator + silver[i];
                seperator = ",";
            }

            intervals += ",";

            seperator = "";
            for (int i = 0; i < bronze.Length; i++)
            {
                intervals += seperator + bronze[i];
                seperator = ",";
            }

            return intervals;
        }
    }

    class States
    {
        public float winnersGold;
        public float winnersSilver;
        public float winnersBronze;
        public float total;

        public States()
        {
            winnersGold = 0;
            winnersSilver = 0;
            winnersBronze = 0;
            total = 0;
        }
    }

    class Analysis
    {
        public string sport;

        public Interval height;
        public Interval age;
        public Interval weight;
        public Interval sex;

        public Dictionary<string, States> countries = new Dictionary<string, States>(); 
        public float totalPopulation;
        public float totalGoldWinners;
        public float totalSilverWinners;
        public float totalBronzeWinners;

        public Analysis()
        {
            height.min = 127;
            height.max = 226;
            height.delta = (227f - 127f) / 10f;
            height.totals = new float[10];
            height.gold = new float[10];
            height.silver = new float[10];
            height.bronze = new float[10];
            height.none = new float[10];

            age.min = 10;
            age.max = 97;
            age.delta = (98f - 10f) / 10f;
            age.totals = new float[10];
            age.gold = new float[10];
            age.bronze = new float[10];
            age.silver = new float[10];
            age.none = new float[10];

            weight.min = 25;
            weight.max = 214;
            weight.delta = (215f - 25f) / 10f;
            weight.totals = new float[10];
            weight.gold = new float[10];
            weight.bronze = new float[10];
            weight.silver = new float[10];
            weight.none = new float[10];

            sex.min = 0;
            sex.max = 1;
            sex.delta = 1;
            sex.totals = new float[2];
            sex.gold = new float[2];
            sex.bronze = new float[2];
            sex.silver = new float[2];
            sex.none = new float[2];
        }
    }

    struct Data
    {
        public float sex;
        public float height;
        public float weight;
        public float age;
    }

    struct Output
    {
        public float gold;
        public float silver;
        public float bronze;
        public float none;
    }

    struct Set
    {
        public Data i;
        public Output o;
    }

    class Program
    {
        static void Main(string[] args)
        {
            string fileUrl = @"F:\Hackathon\athlete_events.csv";

            Dictionary<string, Analysis> sports = new Dictionary<string, Analysis>();
            Dictionary<string, List<Set>> Data = new Dictionary<string, List<Set>>();

            using (StreamReader sr = new StreamReader(fileUrl))
            {
                string line = string.Empty;
                string[] titles = sr.ReadLine().Split(',');
                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    string medal = data[14];

                    string sport = data[12];
                    if (!sports.ContainsKey(sport))
                        sports.Add(sport, new Analysis());

                    Analysis plot = sports[sport];

                    bool sex = data[2] == "M";
                    plot.sex.AddMedal(sex ? 0 : 1, medal);

                    float age;
                    if (float.TryParse(data[3], out age))
                        plot.age.AddMedal(age, medal);

                    float weight;
                    if (float.TryParse(data[5], out weight))
                        plot.weight.AddMedal(weight, medal);

                    float height;
                    if (float.TryParse(data[4], out height))
                        plot.height.AddMedal(height, medal);


                    plot.sport = sport;

                    string NOC = data[7];
                    if (!plot.countries.ContainsKey(NOC))
                        plot.countries.Add(NOC, new States());
                    plot.countries[NOC].total += 1;
                    switch (medal)
                    {
                        case "Gold":  plot.totalGoldWinners += 1; plot.countries[NOC].winnersGold += 1; break;
                        case "Silver": plot.totalSilverWinners += 1; plot.countries[NOC].winnersSilver += 1; break;
                        case "Bronze": plot.totalBronzeWinners += 1; plot.countries[NOC].winnersBronze += 1; break;
                    }
                    plot.totalPopulation += 1;
                }
            }

            string[] keys = sports.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                Analysis plot = sports[keys[i]];
                for (int j = 0; j < plot.age.totals.Length; j++)
                {
                    if (plot.age.totals[j] == 0) continue;

                    plot.age.gold[j] /= plot.age.totals[j];
                    plot.age.bronze[j] /= plot.age.totals[j];
                    plot.age.silver[j] /= plot.age.totals[j];
                    plot.age.none[j] /= plot.age.totals[j];
                }
                for (int j = 0; j < plot.weight.totals.Length; j++)
                {
                    if (plot.weight.totals[j] == 0) continue;

                    plot.weight.gold[j] /= plot.weight.totals[j];
                    plot.weight.bronze[j] /= plot.weight.totals[j];
                    plot.weight.silver[j] /= plot.weight.totals[j];
                    plot.weight.none[j] /= plot.weight.totals[j];
                }
                for (int j = 0; j < plot.height.totals.Length; j++)
                {
                    if (plot.height.totals[j] == 0) continue;

                    plot.height.gold[j] /= plot.height.totals[j];
                    plot.height.bronze[j] /= plot.height.totals[j];
                    plot.height.silver[j] /= plot.height.totals[j];
                    plot.height.none[j] /= plot.height.totals[j];
                }
                for (int j = 0; j < plot.sex.totals.Length; j++)
                {
                    if (plot.sex.totals[j] == 0) continue;

                    plot.sex.gold[j] /= plot.sex.totals[j];
                    plot.sex.bronze[j] /= plot.sex.totals[j];
                    plot.sex.silver[j] /= plot.sex.totals[j];
                    plot.sex.none[j] /= plot.sex.totals[j];
                }
            }

            /*using (StreamWriter fs = new StreamWriter("USA.txt"))
            {
                string ageHeadings = "";
                string weightHeadings = "";
                string heightHeadings = "";
                for (int j = 0; j < 10; j++)
                {
                    ageHeadings += "gold_age_" + j + ",";
                    weightHeadings += "gold_weight_" + j + ",";
                    heightHeadings += "gold_height_" + j + ",";
                }
                for (int j = 0; j < 10; j++)
                {
                    ageHeadings += "silver_age_" + j + ",";
                    weightHeadings += "silver_weight_" + j + ",";
                    heightHeadings += "silver_height_" + j + ",";
                }
                for (int j = 0; j < 10; j++)
                {
                    ageHeadings += "bronze_age_" + j + ",";
                    weightHeadings += "bronze_weight_" + j + ",";
                    heightHeadings += "bronze_height_" + j + ",";
                }

                fs.WriteLine("sport,gold_sex_1,gold_sex_2,silver_sex_1,silver_sex_2,bronze_sex_1,bronze_sex_2," + ageHeadings + weightHeadings + heightHeadings);

                for (int i = 0; i < keys.Length; i++)
                {
                    Analysis plot = sports[keys[i]];

                    fs.WriteLine(keys[i] + "," + plot.sex + "," + plot.age + "," + plot.weight + "," + plot.height);
                }
            }*/

            using (StreamReader sr = new StreamReader(fileUrl))
            {
                string line = string.Empty;
                string[] titles = sr.ReadLine().Split(',');
                while ((line = sr.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    string medal = data[14];

                    string sport = data[12];
                    if (!sports.ContainsKey(sport))
                        sports.Add(sport, new Analysis());

                    Analysis plot = sports[sport];

                    bool sex = data[2] == "M";
                    plot.sex.AddMedal(sex ? 0 : 1, medal);

                    bool valid = true;

                    float age;
                    if (!float.TryParse(data[3], out age))
                        valid = false;
                    
                    float weight;
                    if (!float.TryParse(data[5], out weight))
                        valid = false;

                    float height;
                    if (!float.TryParse(data[4], out height))
                        valid = false;

                    if (valid)
                    {
                        Set set = new Set();

                        set.i.age = age;
                        set.i.weight = weight;
                        set.i.sex = sex ? 0 : 1;
                        set.i.height = height;

                        set.o.gold = (float)Math.Pow(sports[sport].age.GetProbability(age, "Gold") * sports[sport].height.GetProbability(height, "Gold") * sports[sport].sex.GetProbability(sex ? 0 : 1, "Gold") * sports[sport].weight.GetProbability(weight, "Gold"), 1f / 3f);
                        set.o.silver = (float)Math.Pow(sports[sport].age.GetProbability(age, "Silver") * sports[sport].height.GetProbability(height, "Silver") * sports[sport].sex.GetProbability(sex ? 0 : 1, "Silver") * sports[sport].weight.GetProbability(weight, "Silver"), 1f / 3f);
                        set.o.bronze = (float)Math.Pow(sports[sport].age.GetProbability(age, "Bronze") * sports[sport].height.GetProbability(height, "Bronze") * sports[sport].sex.GetProbability(sex ? 0 : 1, "Bronze") * sports[sport].weight.GetProbability(weight, "Bronze"), 1f / 3f);
                        set.o.none = (float)Math.Pow(sports[sport].age.GetProbability(age, "NA") * sports[sport].height.GetProbability(height, "NA") * sports[sport].sex.GetProbability(sex ? 0 : 1, "NA") * sports[sport].weight.GetProbability(weight, "NA"), 1f / 3f);

                        if (!Data.ContainsKey(sport))
                            Data.Add(sport, new List<Set>());
                        Data[sport].Add(set);
                    }
                }
            }

            
            keys = Data.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                List<Set> plot = Data[keys[i]];

                /*using (StreamWriter fs = new StreamWriter(keys[i] + ".csv"))
                {
                    fs.WriteLine("sport,age,weight,height,sex,gold,silver,bronze");

                    for (int j = 0; j < plot.Count; j++)
                    {
                        fs.WriteLine(keys[i] + "," + plot[j].i.age + "," + plot[j].i.weight + "," + plot[j].i.height + "," + plot[j].i.sex + "," + plot[j].o.gold + "," + plot[j].o.silver + "," + plot[j].o.bronze);
                    }
                }*/

                using (StreamWriter fs = new StreamWriter(keys[i] + "_in.txt"))
                {
                    for (int j = 0; j < plot.Count; j++)
                    {
                        fs.Write("[" + "," + plot[j].i.age + "," + plot[j].i.weight + "," + plot[j].i.height + "," + plot[j].i.sex + "],");
                        //fs.WriteLine("[" + plot[j].i.height + "],");
                    }
                }

                using (StreamWriter fs = new StreamWriter(keys[i] + "_out.txt"))
                {
                    for (int j = 0; j < plot.Count; j++)
                    {
                        fs.Write("[" + plot[j].o.gold + "," + plot[j].o.silver + "," + plot[j].o.bronze + "],");
                    }
                }

                using (StreamWriter fs = new StreamWriter(keys[i] + "_medalout.txt"))
                {
                    for (int j = 0; j < plot.Count; j++)
                    {
                        fs.Write("[" + plot[j].o.none + "],");
                    }
                }
            }

            keys = sports.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                Analysis plot = sports[keys[i]];

                using (StreamWriter fs = new StreamWriter(keys[i] + "_Gold.txt"))
                {
                    fs.WriteLine("countries,P(N|X),P(N)");
                    string[] countries = plot.countries.Keys.ToArray();
                    for (int j = 0; j < countries.Length; j++)
                    {
                        if (plot.totalGoldWinners == 0) continue;
                        fs.WriteLine(countries[j] + "," + (plot.countries[countries[j]].winnersGold / plot.totalGoldWinners) + "," + (plot.countries[countries[j]].total / plot.totalPopulation));
                    }
                }

                using (StreamWriter fs = new StreamWriter(keys[i] + "_Silver.txt"))
                {
                    fs.WriteLine("countries,P(N|X),P(N)");
                    string[] countries = plot.countries.Keys.ToArray();
                    for (int j = 0; j < countries.Length; j++)
                    {
                        if (plot.totalSilverWinners == 0) continue;
                        fs.WriteLine(countries[j] + "," + (plot.countries[countries[j]].winnersSilver / plot.totalSilverWinners) + "," + (plot.countries[countries[j]].total / plot.totalPopulation));
                    }
                }

                using (StreamWriter fs = new StreamWriter(keys[i] + "_Bronze.txt"))
                {
                    fs.WriteLine("countries,P(N|X),P(N)");
                    string[] countries = plot.countries.Keys.ToArray();
                    for (int j = 0; j < countries.Length; j++)
                    {
                        if (plot.totalBronzeWinners == 0) continue;
                        fs.WriteLine(countries[j] + "," + (plot.countries[countries[j]].winnersBronze / plot.totalBronzeWinners) + "," + (plot.countries[countries[j]].total / plot.totalPopulation));
                    }
                }

                using (StreamWriter fs = new StreamWriter(keys[i] + "_data.txt"))
                {
                    fs.WriteLine("average participants per year");
                    string[] countries = plot.countries.Keys.ToArray();
                    fs.WriteLine((plot.totalPopulation / 35f));
                }
            }
        }
    }
}
