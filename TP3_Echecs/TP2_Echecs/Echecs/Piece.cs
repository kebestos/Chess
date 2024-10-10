using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    abstract public class Piece
    {
        // attributs
        public InfoPiece info;
        public bool Alive;

        // associations
        public Joueur joueur;
        public Case position;
        public List<Case> listCaseDestination;

        // methodes
        public Piece(Joueur joueur, TypePiece type)
        {
			this.joueur = joueur;
            info = InfoPiece.GetInfo(joueur.couleur, type);
            listCaseDestination = new List<Case>();
            Alive = true;
            //ListDestination();
        }

        public abstract bool Deplacer(Case destination);

        public abstract bool TourDeplacer();

        public abstract void setFirstPosition();

        public abstract bool PionPriseEnPassanr();

        public abstract bool SetFalsePriseEnPassant();

        public abstract bool SetTruePriseEnPassant();

        public abstract bool SetRockTourTrue();
        
        public abstract bool Destination(Case destination);

        public void StopPriseEnPassant()
        {
            foreach (Piece pb in joueur.partie.blancs.pieces)
            {
                pb.SetFalsePriseEnPassant();
            }
            foreach (Piece pn in joueur.partie.noirs.pieces)
            {
                pn.SetFalsePriseEnPassant();
            }
        }
        
        public List<Case> Destiny()
        {
            if (this.position != null)
            {
                List<Case> listCaseD = new List<Case>();
                foreach (Case c in joueur.partie.echiquier.cases)
                {
                    if (this.Destination(c) == true)
                    {
                        listCaseD.Add(c);
                    }
                }
                return listCaseD;
            }
            else return null;
        }

    }
}
