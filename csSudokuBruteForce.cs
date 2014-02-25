    /// <summary>
    /// Brute Force Sudoku Solver
    /// </summary>
    public class csSudokuBruteForce
    {       
        /// <summary>
        /// Class representing each cell in the sudoku grid
        /// </summary>
        protected class csCell
        {
            public int Col { get; private set; }
            public int Row { get; private set; }
            public int Block { get; private set; }
            public Boolean Solved { get; private set; }
            public int Value { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="pIndex">Position of the cell in the 81 char string representing the sudoku</param>
            /// <param name="pValue">Cell value</param>
            public csCell(int pIndex, int pValue)
            {
                Row = pIndex / 9;
                Col = pIndex - Row * 9;
                Block = (Row / 3) * 3 + Col / 3;
                Value = pValue;
                Solved = pValue != 0;
            }

            public override string ToString()
            {
                return string.Format("{0},{1},{2}", Col, Row, Block);
            }
        }

        //cells are stored here
        private List<csCell> grid;

        /// <summary>
        /// Start the brute force solution
        /// </summary>
        public int[] BruteForce(string pData)
        {
            //Populate the grid list
            grid = pData.ToCharArray().Select((itm, ind) => new csCell(ind,Convert.ToInt32(itm)-48)).ToList();

            //linked list of unsolved cells
            LinkedList<csCell> UnsolvedCells =
                new LinkedList<csCell>
                    (
                        grid.Where(c => !c.Solved)
                        .OrderBy(c => c.Row)
                        .ThenBy(c => c.Col)
                        .AsEnumerable()
                    );


            //the current cell being examined
            LinkedListNode<csCell> CurrentCell = UnsolvedCells.Find(grid.First(c => !c.Solved));
            
            do//start the brute force solver
            {
                CurrentCell.Value.Value =
                    UnsolvedCell_GetNextValue(CurrentCell.Value);

                //0 indicates no possible value exists for the current cell
                while (CurrentCell.Value.Value == 0)
                {
                    //get the previous cell..
                    CurrentCell = CurrentCell.Previous;

                    //...and then get it's value
                    CurrentCell.Value.Value =
                            UnsolvedCell_GetNextValue(CurrentCell.Value);
                }

                CurrentCell = CurrentCell.Next;

            } while (CurrentCell != null);// when null, indicates the end of the list has been reached


        }

        //
        //  Get the cell values from the grid generic as an integer array
        //
        public int[] GetGrid()
        {
            //return the solved grid as 1d array
            return grid .OrderBy(c => c.Row)
                        .ThenBy(c => c.Col)
                        .Select(c => c.Value)
                        .ToArray();            
        }

        /// <summary>
        /// Get the next value for the provided unsolved cell
        /// </summary>
        /// <param name="pCell">Cell value is required for</param>
        /// <returns></returns>
        private int UnsolvedCell_GetNextValue(csCell pCell)
        {
            return Enumerable.Range(1, 9).Except
                (
                    //get all the values from neighbours of the provided cell
                    //provided they have a value greater than 0
                    grid.Where(cl => cl.Value > 0 & (cl.Col == pCell.Col | cl.Row == pCell.Row | cl.Block == pCell.Block))
                        .Where(cl=> cl != pCell)
                        .Select(cl => cl.Value)

                ).FirstOrDefault(v => v > pCell.Value);//ensures that if no values are present 0 will be returned
        }
    }
