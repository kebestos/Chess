using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    class Tour : Piece
    {
        public bool FirstPosition;
        public bool Rock;
        public Tour(Joueur joueur) : base(joueur, TypePiece.Tour) {
           this.FirstPosition = true;
           this.Rock = false;
        }

        public override bool Deplacer(Case destination)
        {
            if (destination.piece == null || destination.piece.info.couleur != this.info.couleur)
            {
                bool a = false;
                bool b = false;
                for (int i = min(position.ligne + 1, destination.ligne + 1); i < max(position.ligne, destination.ligne); i++)
                {
                    if (joueur.partie.echiquier.cases[position.colonne, i].piece != null)
                    {
                        a = true;
                        break;
                    }
                }
                for (int i = min(position.colonne + 1, destination.colonne + 1); i < max(position.colonne, destination.colonne); i++)
                {
                    if (joueur.partie.echiquier.cases[i, position.ligne].piece != null)
                    {
                        b = true;
                        break;
                    }
                }
                if (position.ligne !=destination.ligne && position.colonne != destination.colonne) a = true;
                if (a == false && b == false)
                {
                    if (destination.piece != null)
                    {
                        destination.piece.Alive = false;
                        joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                        if (this.joueur.couleur == CouleurCamp.Blanche)
                        {
                            joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                        }
                        else
                        {
                            joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                        }
                    }
                    else
                    {
                        if (Rock == false)
                        {
                            joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                        }
                        else
                        {
                            joueur.partie.AddCoup(this, destination, this.position, null, false, true, false, false);
                            Rock = false;
                        }
                    }
                    destination.Link(this);
                    StopPriseEnPassant();
                    FirstPosition = false;
                    return true;
                }
            }

            return false;
        }     
        

        public override bool TourDeplacer()
        {
            return this.FirstPosition;
        }
        
        public int min(int i, int j)
        {
            if (i > j) return j;
            else return i;
        }

        public int max(int i, int j)
        {
            if (i < j) return j;
            else return i;
        }
        public override bool PionPriseEnPassanr()
        {
            return false;
        }
        public override bool SetTruePriseEnPassant()
        {
            return false;
        }

        public override bool SetFalsePriseEnPassant()
        {
            return false;
        }

        public override bool Destination(Case destination)
        {
            if (destination.piece == null || destination.piece.info.couleur != this.info.couleur)
            {
                bool a = false;
                bool b = false;
                for (int i = min(position.ligne + 1, destination.ligne + 1); i < max(position.ligne, destination.ligne); i++)
                {
                    if (joueur.partie.echiquier.cases[position.colonne, i].piece != null)
                    {
                        a = true;
                        break;
                    }
                }
                for (int i = min(position.colonne + 1, destination.colonne + 1); i < max(position.colonne, destination.colonne); i++)
                {
                    if (joueur.partie.echiquier.cases[i, position.ligne].piece != null)
                    {
                        b = true;
                        break;
                    }
                }
                if (position.ligne != destination.ligne && position.colonne != destination.colonne) a = true;
                if (a == false && b == false)
                {                    
                    return true;
                }
            }

            return false;
        }

        public override void setFirstPosition()
        {
            this.FirstPosition = true;
        }

        public override bool SetRockTourTrue()
        {
            this.Rock = true;
            return Rock;
        }        
    }
}
