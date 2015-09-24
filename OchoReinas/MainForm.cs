using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OchoReinas
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private Int32 tamanioPoblacion=100;
        private Int32 generaciones = 10;
        private double probabilidadDeCruza = 0.70;
        private double probabilidadDeMutacion = 0.01;
        private void btnStart_Click(object sender, EventArgs e)
        {
            GeneticAlgo geneticAlgo = new GeneticAlgo();
            List<Cromosoma> initPopulation = obtenerPoblacionInicial(tamanioPoblacion);
            geneticAlgo.generarRistras(ref initPopulation, generaciones, probabilidadDeCruza, probabilidadDeMutacion);
            
            dgResults.Rows.Clear();
            for (int i = 0; i < initPopulation.Count - 1; i++)
            {
                String sol = "| ";
                for (int j = 0; j < 8; j++)
                {
                    sol = sol + initPopulation[i].genes[j] + " | ";
                }
                dgResults.Rows.Add(new Object[] { sol, initPopulation[i].conveniencia });    
                          
            }
            board1.Genes = initPopulation[0].genes;
        }

       

        private List<Cromosoma> obtenerPoblacionInicial(int population)
        {
            List<Cromosoma> initPop = new List<Cromosoma>();
            GeneticAlgo RandomGen = new GeneticAlgo();
            for (int i = 0; i < population; i++)
            {
                List<int> genes = new List<int>(new int[] {0, 1, 2, 3, 4, 5, 6, 7});
                Cromosoma chromosome = new Cromosoma();
                chromosome.genes = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    int geneIndex = (int)(RandomGen.obtenerAleatorio(0,genes.Count-1)+0.5);
                    chromosome.genes[j] = genes[geneIndex];
                    genes.RemoveAt(geneIndex);
                }

                initPop.Add(chromosome);
            }
            return initPop;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
              
    }
}
