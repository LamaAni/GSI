using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSI.Storage.CSV
{
    /// <summary>
    /// Createds an in memory representation of a csv file.
    /// Dose not require full lines. 
    /// </summary>
    public class CSVMat : IEnumerable<List<string>>
    {
        public CSVMat(string source = "")
        {
            ParseCSV(source);
        }

        #region members

        List<List<string>> m_data = new List<List<string>>();

        /// <summary>
        /// The total number of lines in the csv file.
        /// </summary>
        public int LineCount { get { return m_data.Count; } }

        /// <summary>
        /// Sets or gets the row and col values.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public string this[int row, int col]
        {
            get
            {
                if (m_data.Count <= row || m_data[row].Count <= col)
                {
                    return null;
                }
                return m_data[row][col];
            }
            set
            {
                while (m_data.Count <= row)
                    m_data.Add(new List<string>());

                while (m_data[row].Count <= col)
                    m_data[row].Add(null);

                m_data[row][col] = value;
            }
        }

        /// <summary>
        /// The row associated with the calibration.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public List<string> this[int row]
        {
            get
            {
                if (m_data.Count <= row)
                {
                    m_data.Add(new List<string>());
                }
                return m_data[row];
            }
        }

        #endregion

        #region metrix methods

        public int GetMaxColumn()
        {
            return m_data.Select(l => l.Count).Max();
        }

        #endregion

        #region convertsion methods

        void ParseCSV(string source)
        {
            m_data.Clear();

            // creating the csv 2d ma
            source.Split('\n').Where(l => l.Trim().Length > 0).ToArray().Foreach(l=>{
                m_data.Add(l.Split(',').ToList());
            });
        }

        #endregion

        #region string functions

        /// <summary>
        /// Display the csv as string.
        /// </summary>
        /// <returns></returns>
        public string ToCSVString()
        {
            StringBuilder builder = new StringBuilder();
            m_data.ForEach(l =>
            {
                builder.Append(string.Join(",", l.ToArray()));
                builder.Append("\n");
            });
            return builder.ToString();
        }

        /// <summary>
        /// Create a new csvmatrix for the string source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static CSVMat Parse(string source)
        {
            return new CSVMat(source);
        }
        #endregion

        public IEnumerator<List<string>> GetEnumerator()
        {
            return m_data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
