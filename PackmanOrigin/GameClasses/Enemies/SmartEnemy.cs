using PackmanOrigin.Classes;
using PackmanOrigin.Interfaces.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace PackmanOrigin.GameClasses.Enemies
{
    class SmartEnemy:Enemy 
    {
       
        private Data[,] _DijkstraArray;
        private int _startingRowInd;
        private int _startingColInd;

        private int _currentRowInd;
        private int _currentColInd;

        public SmartEnemy(double leftLocation, double topLocation,int currentRowInd, int currentColInd, string imagePath = "/Assets/Images/RedBlinky (2).jpg") 
            : base(leftLocation, topLocation, imagePath)
        {
            _startingRowInd = currentRowInd;
            _startingColInd = currentColInd;

            _currentRowInd = currentRowInd;
            _currentColInd = currentColInd;
            
        }

        public override void EnemyMovmentLogic(TileObject[,] tileImageArray, Canvas can)
        {
            LeftLocation = Canvas.GetLeft(Image);
            TopLocation = Canvas.GetTop(Image);

            if (LeftLocation == tileImageArray[_currentRowInd, _currentColInd].LeftLocation &&
                TopLocation == tileImageArray[_currentRowInd, _currentColInd].TopLocation)
            {
                BeforeMoveLeft = Canvas.GetLeft(Image);
                BeforeMoveTop = Canvas.GetTop(Image);

                Path path = FindMinPath(_currentRowInd, _currentColInd,PackManCharacter.CurrentRowInd, PackManCharacter.CurrentColInd, tileImageArray);
                if (path.dataPath.First != null)
                {
                    Data nextStep = path.dataPath.First.Value;

                    if (_currentColInd < nextStep.ColInd) EnemyMoveRight();
                    if (_currentRowInd < nextStep.RowInd) EnemyMoveDown();
                    if (_currentColInd > nextStep.ColInd) EnemyMoveLeft();
                    if (_currentRowInd > nextStep.RowInd) EnemyMoveUp();

                    _currentRowInd = nextStep.RowInd;
                    _currentColInd = nextStep.ColInd;
                    return;
                }
              
            }
            LeftLocation = Canvas.GetLeft(Image);
            TopLocation = Canvas.GetTop(Image);

            if (BeforeMoveLeft < LeftLocation) EnemyMoveRight();
            if (BeforeMoveTop < TopLocation) EnemyMoveDown();
            if (BeforeMoveLeft > LeftLocation) EnemyMoveLeft();
            if (BeforeMoveTop > TopLocation) EnemyMoveUp();
        }

        public override void SetToStartLocation()
        {
            Canvas.SetTop(Image, StartTLocation);
            Canvas.SetLeft(Image, StartLLocation);
            _currentRowInd = _startingRowInd;
            _currentColInd = _startingColInd;
        }

        private Path FindMinPath(int sourceRowInd, int sourceColInd, int targetRowInd, int targetColInd, TileObject[,] tileImageArray)
        {
            InitializeDijkstrTable(sourceRowInd, sourceColInd, tileImageArray);


            Data current = GetCurrentData();
         
            while ((current != null) && current.MinTotalWeight < int.MaxValue)
            {
                FindUpdateNeighbours(current,tileImageArray);
                _DijkstraArray[current.RowInd, current.ColInd].Ischecked = true;
                current = GetCurrentData();
               
            }
           return GetMinPath(sourceRowInd,sourceColInd,targetRowInd,targetColInd);
           
        }
        private Data[,] InitializeDijkstrTable(int sourceRowInd,int sourceColInd ,TileObject[,] tileImageArray)
        {  
            _DijkstraArray = new Data[tileImageArray.GetLength(0), tileImageArray.GetLength(1)];
            for (int i = 0; i < _DijkstraArray.GetLength(0); i++)
            {
                for (int j= 0; j < _DijkstraArray.GetLength(1); j++)
                {
                    _DijkstraArray[i,j] = new Data(i,j);
                }
            }
            _DijkstraArray[sourceRowInd, sourceColInd].MinTotalWeight = 0;
            return _DijkstraArray;
        }
        
        private void UpdateNeighbor(Data neighbor, Data current)
        {
            int neighborRowInd = neighbor.RowInd;
            int neighborColInd = neighbor.ColInd;

            int currentRowInd = current.RowInd;
            int currentColInd = current.ColInd;

            if (_DijkstraArray[neighborRowInd, neighborColInd].MinTotalWeight > _DijkstraArray[currentRowInd,currentColInd].MinTotalWeight + 1)
            {
                _DijkstraArray[neighborRowInd, neighborColInd].MinTotalWeight = _DijkstraArray[currentRowInd,currentColInd].MinTotalWeight + 1;
                _DijkstraArray[neighborRowInd, neighborColInd].previousData = current;
            }
        }
        
        private Data GetCurrentData()
        {
            Data current = null;
            int minData = int.MaxValue;
            for (int i = 0; i < _DijkstraArray.GetLength(0); i++)
            {
                for (int j = 0; j < _DijkstraArray.GetLength(1); j++)
                {
                    if(!_DijkstraArray[i, j].Ischecked && _DijkstraArray[i, j].MinTotalWeight < minData )
                    {
                        minData = _DijkstraArray[i, j].MinTotalWeight;
                        current = _DijkstraArray[i, j];
                    }
                }
            }
            return current;
        }

        private void FindUpdateNeighbours(Data current, TileObject[,] tileImageArray)
        {
            // find current neighbours indices, make sure indices are okay(inside board,and not a break type..)
            // Once we update a neighbor - Update its previous data indices to current indices

            //indices of arrayBounds
            int upperBoundInd = 0;
            int downBoundInd= _DijkstraArray.GetLength(0) - 1;
            int rightBoundInd = _DijkstraArray.GetLength(1) - 1;
            int leftBoundInd = 0;

            //current indices
            int rowInd = current.RowInd;
            int columnInd = current.ColInd;

            //neighbors indices
            int rowup = rowInd - 1;
            int rowDown = rowInd + 1;
            int columnLeft = columnInd - 1;
            int columnRight = columnInd + 1;

            //check one tile up
            if (rowup >= upperBoundInd &&
                tileImageArray[rowup, columnInd].GetType() == typeof(BlackTile) &&
                _DijkstraArray[rowup, columnInd].Ischecked == false)
                UpdateNeighbor(_DijkstraArray[rowup, columnInd], current);

            //check one tile down
            if (rowDown <= downBoundInd &&
                tileImageArray[rowDown, columnInd].GetType() == typeof(BlackTile) &&
               _DijkstraArray[rowDown, columnInd].Ischecked == false)
                UpdateNeighbor(_DijkstraArray[rowDown, columnInd], current);

            //check one tile left
            if (columnLeft >= leftBoundInd &&
                tileImageArray[rowInd, columnLeft].GetType() == typeof(BlackTile) &&
                _DijkstraArray[rowInd, columnLeft].Ischecked == false)
                UpdateNeighbor(_DijkstraArray[rowInd, columnLeft], current);

            //check one tile right
            if (columnRight <= rightBoundInd &&
                tileImageArray[rowInd, columnRight].GetType() == typeof(BlackTile) &&
                _DijkstraArray[rowInd, columnRight].Ischecked == false)
                UpdateNeighbor(_DijkstraArray[rowInd, columnRight], current);
        }

         // Dijkstra algorithm Implemention
        private Path GetMinPath(int sourceRowInd, int sourceColInd, int targetRowInd, int targetColInd)
        {
            Path path = new Path();
            path.TotalWeight = _DijkstraArray[targetRowInd, targetColInd].MinTotalWeight;
            while (!(targetRowInd == sourceRowInd && targetColInd == sourceColInd))
            {
                path.dataPath.AddFirst(_DijkstraArray[targetRowInd, targetColInd]);
                int rowInd = targetRowInd;
                int ColInd = targetColInd;

                targetRowInd = _DijkstraArray[rowInd, ColInd].previousData.RowInd;
                targetColInd = _DijkstraArray[rowInd, ColInd].previousData.ColInd;
            }
            return path;
        }

        public class Data 
        {
            public bool Ischecked;
            public  Data previousData { get; set; }
            public int MinTotalWeight;
            public int RowInd{ get; set; }
            public int ColInd { get; set; }

            
            public Data( int rowInd, int colInd)
            {
                MinTotalWeight = int.MaxValue; 
                RowInd = rowInd;
                ColInd = colInd;
            }
        }
        class Path
        {
            public int TotalWeight { get; set; }
            public LinkedList<Data> dataPath { get; set; }
            public Path()
            {
                dataPath = new LinkedList<Data>();
                
                TotalWeight = 0;
            }
        }
    }
   
}

