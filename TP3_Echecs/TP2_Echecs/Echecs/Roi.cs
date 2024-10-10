using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    class Roi : Piece
    {
        bool FirstPosition;
        public Roi(Joueur joueur) : base(joueur, TypePiece.Roi) {
            this.FirstPosition = true;
        }

        public override bool Deplacer(Case destination)
        {
            if (destination.piece == null || destination.piece.info.couleur != this.info.couleur)
            {
               if(AllDestinationPiece(destination) == true)
               {
                    if ((destination.ligne == position.ligne + 1 && destination.colonne == position.colonne) ||
                   (destination.ligne == position.ligne - 1 && destination.colonne == position.colonne) ||
                   (destination.ligne == position.ligne && destination.colonne == position.colonne + 1) ||
                   (destination.ligne == position.ligne && destination.colonne == position.colonne - 1) ||
                   (destination.ligne == position.ligne + 1 && destination.colonne + 1 == position.colonne) ||
                   (destination.ligne == position.ligne + 1 && destination.colonne - 1 == position.colonne) ||
                   (destination.ligne == position.ligne - 1 && destination.colonne - 1 == position.colonne) ||
                   (destination.ligne == position.ligne - 1 && destination.colonne + 1 == position.colonne))
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
                        else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false); ;
                        destination.Link(this);
                        FirstPosition = false;
                        StopPriseEnPassant();
                        return true;
                    }
                    if (destination.ligne == position.ligne && destination.colonne == position.colonne - 2 && FirstPosition == true)
                    {
                        bool RoqueLeft = RoqueLeftOk();
                        if (position.colonne - 4 >= 0 && joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece != null)
                        {
                            if (RoqueLeft && joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece.info.type == TypePiece.Tour &&
                                        joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece.info.couleur == this.info.couleur &&
                                        joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece.TourDeplacer() == true)
                            {
                                joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece.SetRockTourTrue();
                                joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece.Deplacer(joueur.partie.echiquier.cases[position.colonne - 1, position.ligne]);
                                joueur.partie.AddCoup(this, destination, this.position, null, true, false, false, false);
                                destination.Link(this);
                                StopPriseEnPassant();
                                return true;
                            }
                        }                        

                    }
                    if (destination.ligne == position.ligne && destination.colonne == position.colonne + 2 && FirstPosition == true)
                    {
                        bool RoqueRight = RoqueRightOk();
                        if (position.colonne + 3 < 8 && joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece != null)
                        {
                            if (RoqueRight && joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece.info.type == TypePiece.Tour &&
                                        joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece.info.couleur == this.info.couleur &&
                                        joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece.TourDeplacer() == true)
                            {
                                joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece.SetRockTourTrue();
                                joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece.Deplacer(joueur.partie.echiquier.cases[position.colonne + 1, position.ligne]);
                                joueur.partie.AddCoup(this, destination, this.position, null, true, false, false, false);
                                destination.Link(this);                                
                                StopPriseEnPassant();
                                return true;
                            }
                        }                            
                    }
                }               
            }
            return false; 
        }

        public bool RoqueLeftOk()
        {
            for(int i = 1; i < 4; i++)
            {
                if(position.colonne - i >= 0)
                {
                    if (joueur.partie.echiquier.cases[position.colonne - i, position.ligne].piece != null)
                    {
                        return false;
                    }
                }               
            }
            return true;
        }

        public bool RoqueRightOk()
        {
            for (int i = 1; i < 3; i++)
            {
                if (position.colonne + i <= 7)
                {
                    if (joueur.partie.echiquier.cases[position.colonne + i, position.ligne].piece != null)
                    {
                        return false;
                    }
                }
            }
            return true;
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

        public override bool Destination(Case destination)
        {
            if (destination.piece == null || destination.piece.info.couleur != this.info.couleur)
            {
              if (AllDestinationPiece(destination) == true)
              {
                    if ((destination.ligne == position.ligne + 1 && destination.colonne == position.colonne) ||
                    (destination.ligne == position.ligne - 1 && destination.colonne == position.colonne) ||
                    (destination.ligne == position.ligne && destination.colonne == position.colonne + 1) ||
                    (destination.ligne == position.ligne && destination.colonne == position.colonne - 1) ||
                    (destination.ligne == position.ligne + 1 && destination.colonne + 1 == position.colonne) ||
                    (destination.ligne == position.ligne + 1 && destination.colonne - 1 == position.colonne) ||
                    (destination.ligne == position.ligne - 1 && destination.colonne - 1 == position.colonne) ||
                    (destination.ligne == position.ligne - 1 && destination.colonne + 1 == position.colonne))
                    {
                        return true;
                    }
                    if (destination.ligne == position.ligne && destination.colonne == position.colonne - 2 && FirstPosition == true)
                    {
                        bool RoqueLeft = RoqueLeftOk();
                        if (position.colonne - 4 >= 0 && joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece != null)
                        {
                            if (RoqueLeft && joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece.info.type == TypePiece.Tour &&
                                      joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece.info.couleur == this.info.couleur &&
                                      joueur.partie.echiquier.cases[position.colonne - 4, position.ligne].piece.TourDeplacer() == true)
                            {
                                return true;
                            }
                        }

                    }
                    if (destination.ligne == position.ligne && destination.colonne == position.colonne + 2 && FirstPosition == true)
                    {
                        bool RoqueRight = RoqueRightOk();
                        if(position.colonne + 3 < 8 && joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece != null)
                        {
                            if (RoqueRight && joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece.info.type == TypePiece.Tour &&
                                       joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece.info.couleur == this.info.couleur &&
                                       joueur.partie.echiquier.cases[position.colonne + 3, position.ligne].piece.TourDeplacer() == true)
                            {
                                return true;
                            }
                        }                       
                    }
                }
                    
            }
            return false;
        }

        public bool AllDestinationPiece(Case destination)
        {            
            joueur.partie.listCaseDestinationAllPiece();
            if (this.joueur.couleur == CouleurCamp.Blanche)
            {                
                foreach (Case c in joueur.partie.noirs.destinationAllPiece)
                {
                    if(destination == c)
                    {
                        return false;
                    }
                }
                foreach(Piece p in joueur.partie.noirs.pieces)
                {
                    if(p.info.type == TypePiece.Tour || p.info.type == TypePiece.Dame)
                    {
                        if (p.position != null)
                        {
                            if (this.position.colonne == p.position.colonne)
                            {
                                foreach (Case ca in p.listCaseDestination)
                                {
                                    if (this.position == ca)
                                    {
                                        if (position.ligne + 1 < 8)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne, position.ligne + 1])
                                            {
                                                return false;
                                            }
                                        }
                                        if (position.ligne - 1 >= 0)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne, position.ligne - 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                            if(this.position.ligne == p.position.ligne)
                            {
                                foreach (Case ca in p.listCaseDestination)
                                {
                                    if (this.position == ca)
                                    {
                                        if (position.colonne + 1 < 8)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne + 1, position.ligne])
                                            {
                                                return false;
                                            }
                                        }
                                        if (position.colonne - 1 >= 0)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne - 1, position.ligne])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }                            
                    }
                    if (p.info.type == TypePiece.Fou || p.info.type == TypePiece.Dame)
                    {
                        if (p.position != null)
                        {
                            foreach (Case ca in p.listCaseDestination)
                            {
                                if (this.position == ca)
                                {
                                    if (this.position.ligne > p.position.ligne && this.position.colonne > p.position.colonne)
                                    {
                                        if (position.ligne + 1 < 8 && position.colonne + 1 < 8)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne + 1, position.ligne + 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    if (this.position.ligne < p.position.ligne && this.position.colonne < p.position.colonne)
                                    {
                                        if (position.ligne - 1 >= 0 && position.colonne - 1 >= 0)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne - 1, position.ligne - 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    if (this.position.ligne > p.position.ligne && this.position.colonne < p.position.colonne)
                                    {
                                        if (position.ligne + 1 < 8 && position.colonne - 1 >= 0)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne - 1, position.ligne + 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    if (this.position.ligne < p.position.ligne && this.position.colonne > p.position.colonne)
                                    {
                                        if (position.ligne - 1 >= 0 && position.colonne + 1 < 8)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne + 1, position.ligne - 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                            
                    }
                }
            }
            else
            {
                foreach (Case c in joueur.partie.blancs.destinationAllPiece)
                {
                    if (destination == c)
                    {
                        return false;
                    }
                }
                foreach (Piece p in joueur.partie.blancs.pieces)
                {
                    if (p.info.type == TypePiece.Tour || p.info.type == TypePiece.Dame)
                    {
                        
                        if (p.position != null)
                        {
                            if (this.position.colonne == p.position.colonne)
                            {
                                foreach (Case ca in p.listCaseDestination)
                                {                                    
                                    if (this.position == ca)
                                    {                                        
                                        if (position.ligne + 1 < 8)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne, position.ligne + 1])
                                            {
                                                return false;
                                            }
                                        }
                                        if (position.ligne - 1 >= 0)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne, position.ligne - 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                            if (this.position.ligne == p.position.ligne)
                            {
                                foreach (Case ca in p.listCaseDestination)
                                {
                                    if (this.position == ca)
                                    {
                                        if (position.colonne + 1 < 8)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne + 1, position.ligne])
                                            {
                                                return false;
                                            }
                                        }
                                        if (position.colonne - 1 >= 0)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne - 1, position.ligne])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }                        
                    }
                    if (p.info.type == TypePiece.Fou || p.info.type == TypePiece.Dame)
                    {
                        if (p.position != null)
                        {
                            foreach (Case ca in p.listCaseDestination)
                            {
                                if (this.position == ca)
                                {
                                    if (this.position.ligne > p.position.ligne && this.position.colonne > p.position.colonne)
                                    {
                                        if (position.ligne + 1 < 8 && position.colonne + 1 < 8)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne + 1, position.ligne + 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    if (this.position.ligne < p.position.ligne && this.position.colonne < p.position.colonne)
                                    {
                                        if (position.ligne - 1 >= 0 && position.colonne - 1 >= 0)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne - 1, position.ligne - 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    if (this.position.ligne > p.position.ligne && this.position.colonne < p.position.colonne)
                                    {
                                        if (position.ligne + 1 < 8 && position.colonne - 1 >= 0)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne - 1, position.ligne + 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                    if (this.position.ligne < p.position.ligne && this.position.colonne > p.position.colonne)
                                    {
                                        if (position.ligne - 1 >= 0 && position.colonne + 1 < 8)
                                        {
                                            if (destination == joueur.partie.echiquier.cases[position.colonne + 1, position.ligne - 1])
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            
            return true;
        }

        public override bool SetTruePriseEnPassant()
        {
            return false;
        }

        public override bool SetRockTourTrue()
        {
            return false;
        }


        public override void setFirstPosition()
        {
            this.FirstPosition = true;
        }
    }
}
