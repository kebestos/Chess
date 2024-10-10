using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    public class Case
    {
        // attributs
        public int colonne;
        public int ligne;
        //CouleurCamp couleurCase;
        public Partie partie;
        public Piece piece;
        IEvenements vue;

        // associations
        public Case(int ligne, int colonne,/*CouleurCamp couleur*/ Partie partie)
        {
            this.partie = partie;
            this.colonne = colonne;
            this.ligne = ligne;
           // this.couleurCase = couleur;
            this.vue = partie.vue;
            this.piece = null;
        }        

        // methodes
        public void Link(Piece newPiece)
        {
            // 1. Deconnecter newPiece de l'ancienne case
            if(newPiece.position != null)
            {
                UnLink(newPiece);
            }

            // 2. Connecter newPiece à cette case
            vue.ActualiserCase(colonne, ligne, newPiece.info);
            this.piece = newPiece;
            piece.position = this;
            piece.listCaseDestination = piece.Destiny();
                        
        }

        public void UnLink(Piece newPiece)
        {
            newPiece.position.piece = null;
            vue.ActualiserCase(newPiece.position.colonne, newPiece.position.ligne, null);            
        }
    }
}
