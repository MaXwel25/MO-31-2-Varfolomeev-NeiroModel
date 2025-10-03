using System;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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
            int i, j;
            numofneirons = non; // кол-во нейронов текущего слоя
            numofprevneirons = nopn; // кол-во нейронов предыдыдущего слоя
            Neirons = new Neiron[non]; // определение массива нейронов
            name_Layer = nm_Layer; // наименование слоя, который используется
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv"; // csv формт для возможности открытия в Excel

            double[,] Weights; // временный массив синаптических весов текущего слоя

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

            for (i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for (j = 0; j < nopn; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neirons[i] = new Neiron(tmp_weights, nt); // Заполнение массива нейронами
            }
        }
        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            int i, j; // счётчик 
            char[] delim = new char[] { ',', ' ' }; // разделитель слов - ; и "пробел"
            string tmpStr;  // временная строка для чтения 
            string[] tmpStrWeights;  // временный массив строк
            double[,] weights = new double[numofneirons, numofprevneirons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_elemnt;
                    for (i=0; i < numofneirons; i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);
                        for (j=0; j<numofneirons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_elemnt[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                case MemoryMode.SET:
                    string directory = Path.GetDirectoryName(path);

                    string[] linesToWrite = new string[numofneirons]; // строка для записи, каждая строка вес одного нейрона
                    for (i = 0; i < numofneirons; i++) // перебираем все нейроны в текущем слое
                    {
                        string[] weightStrings = new string[numofprevneirons + 1]; // временный массив для хранения весов одного нейрона
                        for (j = 0; j < numofprevneirons + 1; j++)
                        {
                            weightStrings[j] = neirons[i].Weights[j].ToString
                                (System.Globalization.CultureInfo.InvariantCulture); // преобразует число в строку с точкой как разделителем (гарантирует что всегда используется точка)
                        }
                        tmpStr = string.Join(",", weightStrings); // для формирования строки и объединения
                        linesToWrite[i] = tmpStr; // объединение всех весов в одну строку
                    }
                    File.WriteAllLines(path, linesToWrite); // записывает веса в файл
                    break;

                case MemoryMode.INIT:
                    Random rand = new Random(); // инициализация случайными весами
                    for (i = 0; i < numofneirons; i++) // переюор нейронов текущего слоя 
                    {
                        for (j = 0; j < numofprevneirons + 1; j++) // перебор всех весов для каждого нейрона 
                        {
                            weights[i, j] = rand.NextDouble() - 0.5; // инициализация в диапазоне [-0.5, 0.5]
                        }
                    }
                    string initDirectory = Path.GetDirectoryName(path); // ивзлекает путь к папке
                    //f (!Directory.Exists(initDirectory))
                    //   Directory.CreateDirectory(initDirectory); // если её нет или не создалась
                    string[] initLinesToWrite = new string[numofneirons]; // массив строк, по одной каждый нейрона
                    for (i = 0; i < numofneirons; i++)
                    {
                        string[] weightStrings = new string[numofprevneirons + 1]; // массив весов одного нейрона
                        for (j = 0; j < numofprevneirons + 1; j++)
                        {
                            weightStrings[j] = weights[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture); // использование точки как разделителя
                        }
                        initLinesToWrite[i] = string.Join(",", weightStrings); //объединение весов в строку
                    }
                    File.WriteAllLines(path, initLinesToWrite); // запись строки весов (один нейрон) в файл
                    break;
            }
            return weights; // в итоге возвращаем веса
        }

    }
}
