using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    public class Coup
    {
        public Piece piece;
        public Case arriver;
        public Case depart;
        public String coup;
        public Piece deadPiece;
        public StatusPartie state;
        public bool RockL;
        public bool Rock;
        public bool priseEnPassante;
        public bool Promotion;
        public int Index;

        public Coup(Piece piece, Case arriver, Case depart, Piece deadPiece, StatusPartie State, bool RockLeft, bool RockTour, bool PriseEnP,bool Promotion, int Index)
        {
            this.piece = piece;
            this.arriver = arriver;
            this.depart = depart;
            this.coup = CoupString();
            this.deadPiece = deadPiece;
            this.state = State;
            this.RockL = RockLeft;
            this.Rock = RockTour;
            this.priseEnPassante = PriseEnP;
            this.Promotion = Promotion;
            this.Index = Index;
        }

        public String CoupString()
        {
            //Type piece            
            if (depart.piece.info.type == IHM.TypePiece.Cavalier)
            {
                coup += "C ";
            }
            else if (depart.piece.info.type == IHM.TypePiece.Dame)
            {
                coup += "D ";
            }
            else if (depart.piece.info.type == IHM.TypePiece.Roi)
            {
                coup += "R ";
            }
            else if (depart.piece.info.type == IHM.TypePiece.Pion)
            {
                coup += "P ";
            }
            else if (depart.piece.info.type == IHM.TypePiece.Fou)
            {
                coup += "F ";
            }
            else if (depart.piece.info.type == IHM.TypePiece.Tour)
            {
                coup += "T ";
            }
            if (depart.piece.info.couleur == IHM.CouleurCamp.Blanche)
            {
                coup += "B ";
            }
            else coup += "N ";

            //Depart Colonne
            if (depart.colonne == 0) coup += "a";
            if (depart.colonne == 1) coup += "b";
            if (depart.colonne == 2) coup += "c";
            if (depart.colonne == 3) coup += "d";
            if (depart.colonne == 4) coup += "e";
            if (depart.colonne == 5) coup += "f";
            if (depart.colonne == 6) coup += "g";
            if (depart.colonne == 7) coup += "h";
            
            //Depart Ligne
            if (depart.ligne == 0) coup += "8";
            if (depart.ligne == 1) coup += "7";
            if (depart.ligne == 2) coup += "6";
            if (depart.ligne == 3) coup += "5";
            if (depart.ligne == 4) coup += "4";
            if (depart.ligne == 5) coup += "3";
            if (depart.ligne == 6) coup += "2";
            if (depart.ligne == 7) coup += "1";

            coup += " to ";

            //Arriver Colonne
            if (arriver.colonne == 0) coup += "a";
            if (arriver.colonne == 1) coup += "b";
            if (arriver.colonne == 2) coup += "c";
            if (arriver.colonne == 3) coup += "d";
            if (arriver.colonne == 4) coup += "e";
            if (arriver.colonne == 5) coup += "f";
            if (arriver.colonne == 6) coup += "g";
            if (arriver.colonne == 7) coup += "h";

            //Arriver Ligne
            if (arriver.ligne == 0) coup += "8";
            if (arriver.ligne == 1) coup += "7";
            if (arriver.ligne == 2) coup += "6";
            if (arriver.ligne == 3) coup += "5";
            if (arriver.ligne == 4) coup += "4";
            if (arriver.ligne == 5) coup += "3";
            if (arriver.ligne == 6) coup += "2";
            if (arriver.ligne == 7) coup += "1";

            //Eat
            if(arriver.piece != null)
            {

                coup += " : " ;
                if (arriver.piece.info.type == IHM.TypePiece.Cavalier)
                {
                    coup += "C ";
                }
                else if (arriver.piece.info.type == IHM.TypePiece.Dame)
                {
                    coup += "D ";
                }
                else if (arriver.piece.info.type == IHM.TypePiece.Roi)
                {
                    coup += "R ";
                }
                else if (arriver.piece.info.type == IHM.TypePiece.Pion)
                {
                    coup += "P ";
                }
                else if (arriver.piece.info.type == IHM.TypePiece.Fou)
                {
                    coup += "F ";
                }
                else if (arriver.piece.info.type == IHM.TypePiece.Tour)
                {
                    coup += "T ";
                }
                if (arriver.piece.info.couleur == IHM.CouleurCamp.Blanche)
                {
                    coup += "B ";
                }
                else coup += "N ";
               
                coup += "Dead";
            }
            return coup;
        }
    }
}
