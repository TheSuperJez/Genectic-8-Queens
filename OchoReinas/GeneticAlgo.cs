using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OchoReinas
{
    struct Cromosoma
    {
        public int[] genes;
        public int conveniencia;
        public double convenienciaPromedio;
    }

    delegate void Progress(int progress);
    class GeneticAlgo
    {
        private int convenienciaMaxima = 100;
        private Random random;        

        public GeneticAlgo()
        {
            random = new Random((int)DateTime.Now.Ticks);
        }

        public void generarRistras(ref List<Cromosoma> poblacionInicial, int generaciones, double probCruza, double probMutacion)
        {
            int convenienciaTotal = 0;
            evaluarConveniencia(ref poblacionInicial, ref convenienciaTotal);            

            for (int generacion = 0; generacion < generaciones; generacion++)
            {                
                seleccionarPadresACruzar(ref poblacionInicial,convenienciaTotal);
                cruzar(ref poblacionInicial, probCruza);
                mutar(ref poblacionInicial, probMutacion);
                evaluarConveniencia(ref poblacionInicial, ref convenienciaTotal);                
                if (poblacionInicial[poblacionInicial.Count - 1].conveniencia == 100)
                    break;
               
            }
            poblacionInicial.Sort(new ComparadorDeConveniencia());
        }       

        public void cruzar(ref List<Cromosoma> padres, double probabilidad)
        {
            List<Cromosoma> desendencia = new List<Cromosoma>();

            for (int i = 0; i < padres.Count; i++)
            {                
                if (probar(probabilidad))
                {
                    Cromosoma padreX = seleccionarPadresPorProbabilidad(padres);
                    Cromosoma padreY = seleccionarPadresPorProbabilidad(padres);

                    List<int> child = new List<int>();
                    for (int j = 0; j < 8; j++)
                    {
                        if (probar(0.5)) 
                        {
                            for (int k = 0; k < padreX.genes.Length; k++) 
                            {
                                if (!child.Contains(padreX.genes[k]))
                                {
                                    child.Add(padreX.genes[k]);
                                    break;
                                }
                            }
                        }
                        else 
                        {
                            for (int k = 0; k < padreY.genes.Length; k++)
                            {
                                if (!child.Contains(padreY.genes[k]))
                                {
                                    child.Add(padreY.genes[k]);
                                    break;
                                }
                            }
                        }
                    }
                    Cromosoma desen = new Cromosoma();
                    desen.genes = child.ToArray();
                    desendencia.Add(desen);

                }
                else
                {
                    Cromosoma parentX = seleccionarPadresPorProbabilidad(padres);
                    desendencia.Add(parentX);
                }
            }

            while (desendencia.Count > padres.Count)
            {
                desendencia.RemoveAt((int)obtenerAleatorio(0, desendencia.Count - 1));
            }

            padres = desendencia;
        }


        private void seleccionarPadresACruzar(ref List<Cromosoma> padres,int total)
        {
            int totalConveniencia=0;
            for (int i = 0; i < padres.Count; i++)
            {
                totalConveniencia += padres[i].conveniencia;
                Cromosoma temp = padres[i];
                temp.convenienciaPromedio = totalConveniencia / (double)total;
                padres[i] = temp;
            }
        }

        private Cromosoma seleccionarPadresPorProbabilidad(List<Cromosoma> padres)
        {
            Cromosoma seleccion = padres[0];
            double probabilidad = random.NextDouble();
            for (int i = 0; i < padres.Count; i++)
            {
                seleccion = padres[i];
                if (padres[i].convenienciaPromedio > probabilidad)
                    break;
                
            }
            return seleccion;
        }

        public void mutar(ref List<Cromosoma> padres, double probabilidad)
        {
            List<Cromosoma> desendencias = new List<Cromosoma>();   

            for (int i = 0; i < padres.Count; i++)
            {
                Cromosoma desendencia = padres[i];
                for (int mutaPosicion = 0; mutaPosicion < 8; mutaPosicion++)
                {
                    if (probar(probabilidad))
                    {
                        int genIndex = (int)(obtenerAleatorio(0,6)+0.5);
                        if (genIndex>=mutaPosicion)
                        {
                            genIndex += 1;
                        }
                        int cambioTemp = desendencia.genes[mutaPosicion];
                        desendencia.genes[mutaPosicion] = desendencia.genes[genIndex];
                        desendencia.genes[genIndex] = cambioTemp;                       
                    }
                }                                                       
                
                desendencias.Add(desendencia);                
            }

            padres = desendencias;
        }

        public double obtenerAleatorio(double min, double max)
        {
            return min + random.NextDouble() * (max - min);            
        }

        private bool probar(double probabilidad)
        {
            if (random.NextDouble() < probabilidad)
                return true;
            else
                return false;
        }

        public void evaluarConveniencia(ref List<Cromosoma> cromosoma, ref int totalConveniencia)
        {
            int collisions = 0;
            totalConveniencia = 0;
            for (int k = 0; k < cromosoma.Count; k++)
            {
                for (int i = 0; i < cromosoma[k].genes.Length - 1; i++)
                {
                    int x = i;
                    int y = cromosoma[k].genes[i];
                    for (int j = i + 1; j < cromosoma[k].genes.Length; j++)
                    {
                        if (Math.Abs(j - x) == Math.Abs(cromosoma[k].genes[j] - y))
                            collisions++;                        
                    }
                }
                
                Cromosoma temp = cromosoma[k];
                temp.conveniencia = convenienciaMaxima - collisions;
                cromosoma[k] = temp;
                totalConveniencia += cromosoma[k].conveniencia;
                collisions = 0;
            }
        }
    }

    class ComparadorDeConveniencia : Comparer<Cromosoma>
    {
        public override int Compare(Cromosoma x, Cromosoma y)
        {
            if (x.conveniencia == y.conveniencia)
                return 0;
            else if (x.conveniencia < y.conveniencia)
                return 1;
            else
                return -1;
        }
    }
}
