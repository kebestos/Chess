using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_Echecs.Echecs
{
    public class Echiquier
    {
        int colonnes;
        int lignes;
        public Case[,] cases;
        Partie partie;
        public Echiquier(Partie partie)
        {
            this.partie = partie;
            this.colonnes = 8;
            this.lignes = 8;
            this.cases = Affectationcase();
        }

        public Case[,] Affectationcase()
        {
            Case[,] TableauCase = new Case[lignes,colonnes];
            for(int i =0; i < lignes; i++)
            {
                for(int j = 0; j < colonnes; j++)
                {
                    TableauCase[j, i] = new Case(i, j, partie);
                }
            }
            return TableauCase;
        }
    }
}
