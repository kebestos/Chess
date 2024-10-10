using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2_Echecs.IHM;

namespace TP2_Echecs.Echecs
{
    class Pion : Piece
    {
        bool FirstPosition;
        public bool priseEnPassant;
        public Pion(Joueur joueur) : base(joueur, TypePiece.Pion) {
            this.FirstPosition = true;
            this.priseEnPassant = false;
        }

        public override bool Deplacer(Case destination)
        {
            if(FirstPosition == true)
            {
                if (this.info.couleur == CouleurCamp.Blanche)
                {
                    if(destination.piece == null || destination.piece.info.couleur == CouleurCamp.Noire)
                    {
                        if ((destination.ligne == position.ligne - 1 && destination.colonne == position.colonne && destination.piece == null) ||
                            (destination.ligne == position.ligne - 1 && destination.colonne + 1 == position.colonne && destination.piece != null) ||
                            (destination.ligne == position.ligne - 1 && destination.colonne - 1 == position.colonne && destination.piece != null))
                        {
                            if (destination.piece != null)
                            {
                                destination.piece.Alive = false;
                                joueur.partie.AddCoup(this, destination, this.position, destination.piece,false,false,false,false);
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                            destination.Link(this);
                            StopPriseEnPassant();
                            FirstPosition = false;
                            return true;
                        }
                        if (destination.ligne == position.ligne - 2 && destination.colonne == position.colonne && destination.piece == null &&
                            (joueur.partie.echiquier.cases[position.colonne, position.ligne - 1].piece == null))
                        {
                            StopPriseEnPassant();
                            //Console.WriteLine("pp true");
                            this.priseEnPassant = true;
                            joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                            destination.Link(this);
                            FirstPosition = false;
                            return true;
                        }
                    }                    
                    return false;
                }
                else
                {
                    if(destination.piece == null || destination.piece.info.couleur == CouleurCamp.Blanche)
                    {
                       if ((destination.ligne == position.ligne + 1 && destination.colonne == position.colonne && destination.piece == null) ||
                           (destination.ligne == position.ligne + 1 && destination.colonne + 1 == position.colonne && destination.piece != null) ||
                           (destination.ligne == position.ligne + 1 && destination.colonne - 1 == position.colonne && destination.piece != null))
                       {                            
                            if (destination.piece != null)
                            {
                                destination.piece.Alive = false;
                                joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                            else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                            destination.Link(this);
                            StopPriseEnPassant();
                            FirstPosition = false;
                            return true;
                       }
                       if(destination.ligne == position.ligne + 2 && destination.colonne == position.colonne && destination.piece == null &&
                          joueur.partie.echiquier.cases[position.colonne, position.ligne + 1].piece == null)
                       {                           
                            StopPriseEnPassant();
                            //Console.WriteLine("pp true");
                            this.priseEnPassant = true;
                            joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                            destination.Link(this);
                            FirstPosition = false;
                            return true;
                       }                       
                    }                    
                    return false;
                }
            }
            if(this.info.couleur == CouleurCamp.Blanche)
            {
                if(destination.piece == null || destination.piece.info.couleur == CouleurCamp.Noire)
                {
                    if ((destination.ligne == position.ligne - 1 && destination.colonne == position.colonne && destination.piece == null) ||
                       (destination.ligne == position.ligne - 1 && destination.colonne - 1 == position.colonne && destination.piece != null) ||
                       (destination.ligne == position.ligne - 1 && destination.colonne + 1 == position.colonne && destination.piece != null))
                    {
                        
                        if (destination.ligne == 0)
                        {
                            if (destination.piece != null)
                            {
                                destination.piece.Alive = false;
                                joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, true);
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, true);
                            joueur.partie.echiquier.cases[position.colonne, position.ligne].UnLink(this);
                            joueur.pieces.Remove(this);
                            joueur.pieces.Add(new Dame(joueur));
                            joueur.partie.echiquier.cases[destination.colonne, destination.ligne].Link(joueur.pieces[15]);
                            StopPriseEnPassant();
                            return true;
                        }
                        else
                        {
                            if (destination.piece != null)
                            {
                                destination.piece.Alive = false;
                                joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                                joueur.partie.noirs.deadPiece.Add(destination.piece.info);
                            }
                            else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                            destination.Link(this);
                            StopPriseEnPassant();
                            return true;
                        }                        
                    }

                    //Prise En Passant
                    if(destination.ligne == position.ligne - 1 && destination.colonne + 1 == position.colonne && destination.piece == null)
                    {
                        if(joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece != null &&
                           joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.PionPriseEnPassanr() == true &&
                           joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.info.couleur != this.info.couleur)
                        {
                            joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece, false, false, true, false);
                            joueur.partie.noirs.deadPiece.Add(joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.info);
                            joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].UnLink(joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece);
                            destination.Link(this);
                            StopPriseEnPassant();
                            //Console.WriteLine("pion blanc vers la gauche");
                            return true;
                        }
                    }
                    if (destination.ligne == position.ligne - 1 && destination.colonne - 1 == position.colonne && destination.piece == null)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece != null &&
                           joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.PionPriseEnPassanr() == true &&
                           joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.info.couleur != this.info.couleur)
                        {
                            joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece, false, false, true, false);
                            joueur.partie.noirs.deadPiece.Add(joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.info);
                            joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].UnLink(joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece);
                            destination.Link(this);
                            StopPriseEnPassant();
                            //Console.WriteLine("pion blanc vers la droite");
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                if(destination.piece == null || destination.piece.info.couleur == CouleurCamp.Blanche)
                {
                    if ((destination.ligne == position.ligne + 1 && destination.colonne == position.colonne && destination.piece == null) ||
                       (destination.ligne == position.ligne + 1 && destination.colonne - 1 == position.colonne && destination.piece != null) ||
                       (destination.ligne == position.ligne + 1 && destination.colonne + 1 == position.colonne && destination.piece != null))
                    {
                        if (destination.ligne == 7)
                        {
                            if (destination.piece != null)
                            {
                                destination.piece.Alive = false;
                                joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, true);
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                            else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, true);
                            joueur.partie.echiquier.cases[position.colonne, position.ligne].UnLink(this);
                            joueur.pieces.Remove(this);
                            joueur.pieces.Add(new Dame(joueur));
                            joueur.partie.echiquier.cases[destination.colonne, destination.ligne].Link(joueur.pieces[15]);
                            StopPriseEnPassant();
                            return true;
                        }
                        else
                        {                           
                            if (destination.piece != null)
                            {
                                destination.piece.Alive = false;
                                joueur.partie.AddCoup(this, destination, this.position, destination.piece, false, false, false, false);
                                joueur.partie.blancs.deadPiece.Add(destination.piece.info);
                            }
                            else joueur.partie.AddCoup(this, destination, this.position, null, false, false, false, false);
                            destination.Link(this);
                            StopPriseEnPassant();
                            return true;
                        }
                    }
                    //Prise En Passant
                    if (destination.ligne == position.ligne + 1 && destination.colonne + 1 == position.colonne && destination.piece == null)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece != null &&
                           joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.PionPriseEnPassanr() == true &&
                           joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.info.couleur != this.info.couleur)
                        {
                            joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece, false, false, true, false);
                            joueur.partie.blancs.deadPiece.Add(joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.info);
                            joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].UnLink(joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece);
                            destination.Link(this);
                            //Console.WriteLine("pion noir vers la gauche");
                            StopPriseEnPassant();
                            return true;
                        }
                    }
                    if (destination.ligne == position.ligne + 1 && destination.colonne - 1 == position.colonne && destination.piece == null)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece != null &&
                           joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.PionPriseEnPassanr() == true &&
                           joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.info.couleur != this.info.couleur)
                        {
                            joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.Alive = false;
                            joueur.partie.AddCoup(this, destination, this.position, joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece, false, false, true, false);
                            joueur.partie.blancs.deadPiece.Add(joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.info);
                            joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].UnLink(joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece);
                            destination.Link(this);
                            //Console.WriteLine("pion noir vers la droite");
                            StopPriseEnPassant();
                            return true;
                        }
                    }
                }                
                return false;
            }
        }
        public override bool TourDeplacer()
        {
            return false;
        }

        public override bool PionPriseEnPassanr()
        {
            return this.priseEnPassant;
        }

        public override void setFirstPosition()
        {
            FirstPosition = true;
        }

        public override bool SetTruePriseEnPassant()
        {
            this.priseEnPassant = true;
            return this.priseEnPassant;
        }

        public override bool SetRockTourTrue()
        {
            return false;
        }

        public override bool SetFalsePriseEnPassant()
        {
            this.priseEnPassant = false;
            //Console.WriteLine(priseEnPassant.ToString());
            return this.priseEnPassant;
        }

        public override bool Destination(Case destination)
        {
            if (FirstPosition == true)
            {
                if (this.info.couleur == CouleurCamp.Blanche)
                {
                    if (destination.piece == null || destination.piece.info.couleur == CouleurCamp.Noire)
                    {
                        if ((destination.ligne == position.ligne - 1 && destination.colonne == position.colonne && destination.piece == null) ||
                            (destination.ligne == position.ligne - 1 && destination.colonne + 1 == position.colonne && destination.piece != null) ||
                            (destination.ligne == position.ligne - 1 && destination.colonne - 1 == position.colonne && destination.piece != null))
                        {
                            return true;
                        }
                        if (destination.ligne == position.ligne - 2 && destination.colonne == position.colonne && destination.piece == null &&
                            (joueur.partie.echiquier.cases[position.colonne, position.ligne - 1].piece == null))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    if (destination.piece == null || destination.piece.info.couleur == CouleurCamp.Blanche)
                    {
                        if ((destination.ligne == position.ligne + 1 && destination.colonne == position.colonne && destination.piece == null) ||
                            (destination.ligne == position.ligne + 1 && destination.colonne + 1 == position.colonne && destination.piece != null) ||
                            (destination.ligne == position.ligne + 1 && destination.colonne - 1 == position.colonne && destination.piece != null))
                        {
                            
                            return true;
                        }
                        if (destination.ligne == position.ligne + 2 && destination.colonne == position.colonne && destination.piece == null &&
                           joueur.partie.echiquier.cases[position.colonne, position.ligne + 1].piece == null)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            if (this.info.couleur == CouleurCamp.Blanche)
            {
                if (destination.piece == null || destination.piece.info.couleur == CouleurCamp.Noire)
                {
                    if ((destination.ligne == position.ligne - 1 && destination.colonne == position.colonne && destination.piece == null) ||
                       (destination.ligne == position.ligne - 1 && destination.colonne - 1 == position.colonne && destination.piece != null) ||
                       (destination.ligne == position.ligne - 1 && destination.colonne + 1 == position.colonne && destination.piece != null))
                    {

                        if (destination.ligne == 0)
                        {
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    //Prise En Passant
                    if (destination.ligne == position.ligne - 1 && destination.colonne + 1 == position.colonne && destination.piece == null)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece != null &&
                           joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.PionPriseEnPassanr() == true &&
                           joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.info.couleur != this.info.couleur)
                        {
                            
                            return true;
                        }
                    }
                    if (destination.ligne == position.ligne - 1 && destination.colonne - 1 == position.colonne && destination.piece == null)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece != null &&
                           joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.PionPriseEnPassanr() == true &&
                           joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.info.couleur != this.info.couleur)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                if (destination.piece == null || destination.piece.info.couleur == CouleurCamp.Blanche)
                {
                    if ((destination.ligne == position.ligne + 1 && destination.colonne == position.colonne && destination.piece == null) ||
                       (destination.ligne == position.ligne + 1 && destination.colonne - 1 == position.colonne && destination.piece != null) ||
                       (destination.ligne == position.ligne + 1 && destination.colonne + 1 == position.colonne && destination.piece != null))
                    {
                        if (destination.ligne == 7)
                        {
                            
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    //Prise En Passant
                    if (destination.ligne == position.ligne + 1 && destination.colonne + 1 == position.colonne && destination.piece == null)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece != null &&
                           joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.PionPriseEnPassanr() == true &&
                           joueur.partie.echiquier.cases[position.colonne - 1, position.ligne].piece.info.couleur != this.info.couleur)
                        {
                            return true;
                        }
                    }
                    if (destination.ligne == position.ligne + 1 && destination.colonne - 1 == position.colonne && destination.piece == null)
                    {
                        if (joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece != null &&
                           joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.PionPriseEnPassanr() == true &&
                           joueur.partie.echiquier.cases[position.colonne + 1, position.ligne].piece.info.couleur != this.info.couleur)
                        {
                            
                            return true;
                        }
                    }
                }
                return false;
            }
        }

    }
}
