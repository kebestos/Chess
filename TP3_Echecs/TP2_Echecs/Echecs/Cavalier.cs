using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    class Cavalier : Piece
    {
        public Cavalier(Joueur joueur) : base(joueur, TypePiece.Cavalier) { }

        public override bool Deplacer(Case destination)
        {
            if (destination.piece == null || destination.piece.info.couleur != this.info.couleur)
            {
                if ((destination.ligne == position.ligne - 2 && destination.colonne == position.colonne + 1) ||
                    (destination.ligne == position.ligne - 2 && destination.colonne == position.colonne - 1) ||
                    (destination.ligne == position.ligne + 2 && destination.colonne == position.colonne + 1) ||
                    (destination.ligne == position.ligne + 2 && destination.colonne == position.colonne - 1) ||
                    (destination.ligne == position.ligne + 1 && destination.colonne == position.colonne - 2) ||
                    (destination.ligne == position.ligne + 1 && destination.colonne == position.colonne + 2) ||
                    (destination.ligne == position.ligne - 1 && destination.colonne == position.colonne - 2) ||
                    (destination.ligne == position.ligne - 1 && destination.colonne == position.colonne + 2))
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
                    else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                    destination.Link(this);
                    StopPriseEnPassant();
                    return true;
                }               
            }
            return false;
        }
        public override bool TourDeplacer()
        {
            return false;
        }
        public override bool PionPriseEnPassanr()
        {
            return false;
        }

        public override bool SetFalsePriseEnPassant()
        {
            return false;
        }

        public override bool SetTruePriseEnPassant()
        {
            return false;
        }

        public override void setFirstPosition() { }

        public override bool SetRockTourTrue()
        {
            return false;
        }

        public override bool Destination(Case destination)
        {
            if (destination.piece == null || destination.piece.info.couleur != this.info.couleur)
            {
                if ((destination.ligne == position.ligne - 2 && destination.colonne == position.colonne + 1) ||
                    (destination.ligne == position.ligne - 2 && destination.colonne == position.colonne - 1) ||
                    (destination.ligne == position.ligne + 2 && destination.colonne == position.colonne + 1) ||
                    (destination.ligne == position.ligne + 2 && destination.colonne == position.colonne - 1) ||
                    (destination.ligne == position.ligne + 1 && destination.colonne == position.colonne - 2) ||
                    (destination.ligne == position.ligne + 1 && destination.colonne == position.colonne + 2) ||
                    (destination.ligne == position.ligne - 1 && destination.colonne == position.colonne - 2) ||
                    (destination.ligne == position.ligne - 1 && destination.colonne == position.colonne + 2))
                {
                    
                    return true;
                }
            }
            return false;
        }
    }
}
