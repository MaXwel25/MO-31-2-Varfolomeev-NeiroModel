using MO_31_2_Varfolomeev_NeiroModel;
using System;
using System.IO;
using System.Configuration;
using System.Windows.Forms;


namespace MO_31_2_Varfolomeev_NeiroModel.NeiroNet
{
    abstract class Layer
    {
        protected string name_Layer; // наименование слоя
        string pathDirWeights; // путь к катологу гдк находится файл
        string pathFileWeights; // путь к файлу синаптических весов
        protected int numofneirons; // число нейронов текущего слоя
        protected int numofprevneirons; // число нейронов предыдущего слоя
        protected const double learningrate = 0.060; // скорость обучения
        protected const double momentum = 0.050d; // момент инерции
        protected double[,] lastdeltaweights; // веса предыдущей итерации
        protected Neiron[] neirons; // массив нейронов текущего слоя

        // свойства
        public Neiron[] Neirons { get => neirons; set => neirons = value; }

        // активация нейрона
        public double[] Data
        {
            set
            {
                for (int i = 0; i < numofneirons; i++)
                    Neirons[i].Activator(value);
            }
        }
        
        // конструктор
        protected Layer(int non, int nopn, NeironType nt, string nm_Layer)
        {
            numofneirons = non; // кол-во нейронов текущего слоя
            numofprevneirons = nopn; // кол-во нейронов предыдыдущего слоя
            Neirons = new Neiron[non]; // определение массива нейронов
            name_Layer = nm_Layer; // наименование слоя, который используется
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            double[,] Weights; //временный массив синаптических весов
            lastdeltaweights = new double[non, nopn + 1];

            if (File.Exists(pathFileWeights)) // определяет существует ли path
            {
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            }
            else
            {
                Directory.CreateDirectory(pathFileWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            for (int i = 0; i < non; i++) // цикл формирования нейронов слоя и заполнения
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neirons[i] = new Neiron(tmp_weights, nt); // Заполнение массива нейронами
            }
        }
        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' }; // разделитель слов - ; и "пробел"
            string[] tmpStrWeights;  // временный массив строк
            double[,] weights = new double[numofneirons, numofprevneirons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_elemnt;
                    for (int i = 0; i < numofneirons; i++) // строка весов нейрона
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim); // разбивает строку
                        for (int j = 0; j < numofneirons + 1; j++) // каждый отдельный вес нейрона
                        {
                            weights[i, j] = double.Parse(memory_elemnt[j].Replace(",", "."),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                case MemoryMode.SET:
                    tmpStrWeights = new string[numofneirons]; // создаём строку из весов нейрона (tmpStrWeights это массив, где каждый i-ый элемент это строка весов) 
                    for (int i = 0; i < numofneirons; i++)
                    {
                        string[] memory_elemnt2 = new string[numofprevneirons + 1];
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            memory_elemnt2[j] = neirons[i].Weights[j]
                                .ToString(System.Globalization.CultureInfo.InvariantCulture)
                                .Replace('.', ',');
                        }
                        tmpStrWeights[i] = string.Join(";", memory_elemnt2);
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;


                case MemoryMode.INIT:
                    Random random = new Random();
                    for (int i = 0; i < numofneirons; i++)
                    {
                        double sum = 0.0;
                        double squaredSum = 0.0;

                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            // диапазон [-1, +1]
                            weights[i, j] = random.NextDouble() * 2.0 - 1.0;

                            sum += weights[i, j];
                            squaredSum += weights[i, j] * weights[i, j];
                        }

                        double mean = sum / (numofprevneirons + 1);
                        double variance = (squaredSum / (numofprevneirons + 1)) - (mean * mean);
                        double root = Math.Sqrt(variance);

                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            weights[i, j] = (weights[i, j] - mean) / root;
                        }
                    }

                    // сохраняем weights в csv
                    string[] lines = new string[numofneirons];
                    for (int i = 0; i < numofneirons; i++)
                    {
                        string[] row = new string[numofprevneirons + 1];
                        for (int j = 0; j < numofprevneirons + 1; j++)
                        {
                            row[j] = weights[i, j]
                                .ToString(System.Globalization.CultureInfo.InvariantCulture)
                                .Replace('.', ',');
                        }
                        lines[i] = string.Join(";", row);
                    }
                    File.WriteAllLines(path, lines);
                    break;
            }
                 return weights; // в итоге возвращаем веса
        }




    }
}
