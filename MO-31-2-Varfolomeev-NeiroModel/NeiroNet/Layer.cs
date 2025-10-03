using System;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace MO_31_1_Varfolomeev_NeiroModel.NeiroNet
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


        public Neiron[] Neirons { get => neirons; set => neirons = value; }

        public double[] Data
        {
            set
            {
                for (int i = 0; i < numofneirons; i++)
                    Neirons[i].Activator(value);
            }
        }
        /*
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
            public double WeightInitialize(MemoryMode mm, string path)
            {
            int i, j;
            char[] delim = new char[] { " ;"," "};
            string tmpStr;
            string[] = 

            }
        // дописать два свича
        */
    }
}
