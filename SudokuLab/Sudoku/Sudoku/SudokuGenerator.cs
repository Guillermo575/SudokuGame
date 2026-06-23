using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
namespace Sudoku
{
    [Serializable]
    public class SudokuGenerator
    {
        #region Variables
        public int ColumnasX = 3;
        public int ColumnasY = 3;
        public List<Celda> lstCeldas;
        public int ValorInicial { private set; get; } = 1;
        public int ValorFinal { get { return ColumnasX * ColumnasY; } }
        public int SumaCuadrantes { get { return ColumnasX * ColumnasY; } }
        public int SumaCeldas { get { return (ColumnasX * ColumnasX) * (ColumnasY * ColumnasY); } }
        public int SumaBases { get { return ((ValorFinal + 1) * ValorInicial) / 2; } }
        private List<Bitacora> lstBitacora = new List<Bitacora>();
        private List<Bitacora> lstBitacoraBloqueo = new List<Bitacora>();
        private Random rnd = new Random();
        private Dictionary<int, int> mapeoValores;
        public bool Exito { private set; get; }
        public bool Validado { private set; get; }
        public int ConteoErrores { private set; get; }
        public int ConteoAciertos { private set; get; }
        public long TiempoEjecutado { private set; get; }
        public string HashSudoku { private set; get; }
        #endregion

        #region HTML Table
        public String ResumenHTML
        {
            get
            {
                String Tabla = "";
                Tabla += "<table>";
                for (int m = 0; m < ColumnasY; m++)
                {
                    for (int y = 0; y < ColumnasX; y++)
                    {
                        Tabla += "<tr>";
                        for (int l = 0; l < ColumnasX; l++)
                        {
                            for (int x = 0; x < ColumnasY; x++)
                            {
                                try
                                {
                                    var objCelda = (from obj in lstCeldas where obj.EjeX == x && obj.CuadranteEjeX == l && obj.EjeY == y && obj.CuadranteEjeY == m select obj).ToList().First();
                                    bool esColorAlterno = (objCelda.CuadranteEjeX + objCelda.CuadranteEjeY) % 2 == 0;
                                    Tabla += $"<td {(esColorAlterno ? "style='background-color:#DDD'" : "style='background-color:#FFF'")}> {Alphabet.getAlphaChar(objCelda.Valor)} </td>";
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error al generar la tabla HTML: {ex.Message}");
                                    return "Error al generar la tabla HTML";
                                }
                            }
                        }
                        Tabla += "</tr>";
                    }
                }
                Tabla += "</table>";
                return Tabla;
            }
        }
        #endregion

        #region ASCII Table
        public String ResumenASCII
        {
            get
            {
                try
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    int totalFilas = ColumnasY * ColumnasX;
                    int totalColumnas = ColumnasX * ColumnasY;
                    sb.Append("╔");
                    for (int col = 0; col < totalColumnas; col++)
                    {
                        sb.Append("═══");
                        if (col < totalColumnas - 1)
                        {
                            sb.Append((col + 1) % ColumnasY == 0 ? "╦" : "╤");
                        }
                    }
                    sb.AppendLine("╗");
                    for (int m = 0; m < ColumnasY; m++)
                    {
                        for (int y = 0; y < ColumnasX; y++)
                        {
                            sb.Append("║");
                            for (int l = 0; l < ColumnasX; l++)
                            {
                                for (int x = 0; x < ColumnasY; x++)
                                {
                                    var objCelda = (from obj in lstCeldas
                                                    where obj.EjeX == x && obj.CuadranteEjeX == l &&
                                                          obj.EjeY == y && obj.CuadranteEjeY == m
                                                    select obj).ToList().First();

                                    string valor = Alphabet.getAlphaChar(objCelda.Valor);
                                    sb.Append($" {valor} ");

                                    if (x < ColumnasY - 1 || l < ColumnasX - 1)
                                    {
                                        sb.Append((x == ColumnasY - 1) ? "║" : "│");
                                    }
                                }
                            }
                            sb.AppendLine("║");
                            if (y < ColumnasX - 1 || m < ColumnasY - 1)
                            {
                                if (y == ColumnasX - 1 && m < ColumnasY - 1)
                                {
                                    sb.Append("╠");
                                    for (int col = 0; col < totalColumnas; col++)
                                    {
                                        sb.Append("═══");
                                        if (col < totalColumnas - 1)
                                        {
                                            sb.Append((col + 1) % ColumnasY == 0 ? "╬" : "╪");
                                        }
                                    }
                                    sb.AppendLine("╣");
                                }
                                else if (y < ColumnasX - 1)
                                {
                                    sb.Append("╟");
                                    for (int col = 0; col < totalColumnas; col++)
                                    {
                                        sb.Append("───");
                                        if (col < totalColumnas - 1)
                                        {
                                            sb.Append((col + 1) % ColumnasY == 0 ? "╫" : "┼");
                                        }
                                    }
                                    sb.AppendLine("╢");
                                }
                            }
                        }
                    }
                    sb.Append("╚");
                    for (int col = 0; col < totalColumnas; col++)
                    {
                        sb.Append("═══");
                        if (col < totalColumnas - 1)
                        {
                            sb.Append((col + 1) % ColumnasY == 0 ? "╩" : "╧");
                        }
                    }
                    sb.AppendLine("╝");
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al generar la tabla ASCII: {ex.Message}");
                    return "Error al generar la tabla ASCII";
                }
            }
        }
        #endregion

        #region	Test
        public static void Testeo(int cantidad = 50000)
        {
            List<SudokuGenerator> lst = new List<SudokuGenerator>();
            int ValorMinimo = 0;
            int ValorMaximo = 0;
            double ValorPromed = 0;
            double ValorPromedReit = 0;
            long TiempoMinimo = 0;
            long TiempoMaximo = 0;
            long TiempoTotal = 0;
            long TiempoPromedio = 0;
            for (int l = 0; l < cantidad; l++)
            {
                SudokuGenerator pr = new SudokuGenerator();
                if (pr.Exito)
                {
                    lst.Add(pr);
                    ValorMinimo = lst.Min(x => x.ConteoErrores);
                    ValorMaximo = lst.Max(x => x.ConteoErrores);
                    ValorPromed = lst.Average(x => x.ConteoErrores);
                    ValorPromedReit = l == 0 ? pr.ConteoErrores : (ValorPromedReit + pr.ConteoErrores) / 2;
                    TiempoMinimo = lst.Min(x => x.TiempoEjecutado);
                    TiempoMaximo = lst.Max(x => x.TiempoEjecutado);
                    TiempoTotal = lst.Sum(x => x.TiempoEjecutado);
                    TiempoPromedio = TiempoTotal / lst.Count;
                }
            }
            var TodosExitos = (from x in lst where x.Exito select x).Count() == cantidad;
            double minutos = TiempoTotal / 60000.0;
        }
        #endregion

        #region General
        public SudokuGenerator(int ColumnasX = 3, int ColumnasY = 3)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.ColumnasX = ColumnasX;
            this.ColumnasY = ColumnasY;
            SetNewArray();
            SetDatos();
            Validado = true;
            foreach (var obj in lstCeldas)
            {
                Validado = Validado ? ValidoSoloUno(obj) : false;
                obj.lstWarnings = null;
            }
            stopwatch.Stop();
            TiempoEjecutado = stopwatch.ElapsedMilliseconds;
        }
        public static bool ValidarCeldas(List<Celda> lstCeldas)
        {
            bool Validado = true;
            foreach (var objCelda in lstCeldas)
            {
                if (!Validado || objCelda.Valor == 0)
                {
                    return false;
                }
                Validado = (from x in lstCeldas
                            where ((x.IdCuadrante == objCelda.IdCuadrante) ||
                                                       (x.CuadranteEjeX == objCelda.CuadranteEjeX && x.EjeX == objCelda.EjeX) ||
                                                       (x.CuadranteEjeY == objCelda.CuadranteEjeY && x.EjeY == objCelda.EjeY)) &&
                                                        x.Valor == objCelda.Valor
                            select x).ToList().Count == 1;
            }
            return Validado;
        }
        #endregion

        #region Process
        public List<Celda> GetCeldasValidas(int Cuadrante, int Valor)
        {
            List<Celda> lstRetorno = new List<Celda>();
            lstRetorno = (from x in lstCeldas where x.IdCuadrante == Cuadrante && x.Valor == 0 && ValidoEjeXY(x, Valor) && !GetBloqueo(x, Valor) select x).ToList();
            return lstRetorno;
        }
        public bool ValidoSoloUno(Celda objCelda)
        {
            int Valor = objCelda.Valor;
            var lstRetorno = (from x in lstCeldas
                              where ((x.IdCuadrante == objCelda.IdCuadrante) ||
                                                         (x.CuadranteEjeX == objCelda.CuadranteEjeX && x.EjeX == objCelda.EjeX) ||
                                                         (x.CuadranteEjeY == objCelda.CuadranteEjeY && x.EjeY == objCelda.EjeY)) &&
                                                          x.Valor == Valor
                              select x).ToList();
            return lstRetorno.Count == 1;
        }
        public bool ValidoEjeXY(Celda objCelda, int Valor)
        {
            var lstRetorno = (from x in lstCeldas
                              where ((x.CuadranteEjeX == objCelda.CuadranteEjeX && x.EjeX == objCelda.EjeX) ||
                                                         (x.CuadranteEjeY == objCelda.CuadranteEjeY && x.EjeY == objCelda.EjeY)) &&
                                                          x.Valor == Valor
                              select x).ToList();
            return lstRetorno.Count == 0;
        }
        public bool GetBloqueo(Celda objCelda, int Valor)
        {
            var lstBloqueo = (from x in lstBitacoraBloqueo where x.Bloque == objCelda && x.Valor == Valor select x).ToList();
            return lstBloqueo.Count > 0;
        }
        private void SetNewArray()
        {
            lstCeldas = new List<Celda>();
            int IdCuadrante = 1;
            int IdCelda = 1;
            for (int m = 0; m < ColumnasY; m++)
            {
                for (int l = 0; l < ColumnasX; l++)
                {
                    for (int y = 0; y < ColumnasX; y++)
                    {
                        for (int x = 0; x < ColumnasY; x++)
                        {
                            lstCeldas.Add(new Celda(IdCelda, IdCuadrante, l, m, x, y));
                            IdCelda++;
                        }
                    }
                    IdCuadrante++;
                }
            }
        }
        private void SetDatos()
        {
            int celdaGenerada = 0;
            int ValorActual = ValorInicial;
            while (ValorActual <= ValorFinal)
            {
                int CuadranteIndex = 1;
                while (CuadranteIndex <= SumaCuadrantes)
                {
                    var lstCeldasValidas = GetCeldasValidas(CuadranteIndex, ValorActual);
                    if (lstCeldasValidas.Count > 0)
                    {
                        var objCeldaElegida = lstCeldasValidas.Count == 1 ? lstCeldasValidas.First() : GetCeldaElegida(lstCeldasValidas);
                        objCeldaElegida.Valor = ValorActual;
                        lstBitacora.Add(new Bitacora() { Bloque = objCeldaElegida, Valor = ValorActual });
                        if (lstBitacoraBloqueo.Count > 0)
                        {
                            lstBitacoraBloqueo = (from x in lstBitacoraBloqueo where (x.Valor < ValorActual) || (x.Valor == ValorActual && x.Bloque.IdCuadrante <= CuadranteIndex) select x).ToList();
                        }
                        CuadranteIndex++;
                        celdaGenerada++;
                    }
                    else
                    {
                        ConteoErrores++;
                        var objLast = lstBitacora.Last();
                        objLast.Bloque.Valor = 0;
                        lstBitacora.RemoveAt(lstBitacora.Count - 1);
                        var objNewLast = lstBitacora.Last().Bloque;
                        var existeWarning = (from x in objNewLast.lstWarnings where x.Bloque.Id == objLast.Bloque.Id select x).ToList();
                        if (existeWarning.Count > 0)
                        {
                            existeWarning.First().Valor++;
                        }
                        else
                        {
                            objNewLast.lstWarnings.Add(new Bitacora { Bloque = objLast.Bloque, Valor = 1 });
                        }
                        CuadranteIndex--;
                        celdaGenerada--;
                        if (CuadranteIndex <= 0)
                        {
                            ValorActual--;
                            CuadranteIndex = SumaCuadrantes;
                        }
                        if (ValorActual <= 0)
                        {
                            return;
                        }
                        lstBitacoraBloqueo.Add(new Bitacora() { Bloque = objLast.Bloque, Valor = ValorActual });
                    }
                    ConteoAciertos += celdaGenerada > ConteoAciertos ? 1 : 0;
                }
                ValorActual++;
            }
            //HashSudoku = GenerarHashSudoku();
            InicializarMapeoValores();
            Exito = ValidarCeldas(lstCeldas);
            HashSudoku = GenerarHashSudoku();
        }
        public Celda GetCeldaElegida(List<Celda> lst)
        {
            if (lstBitacora.Count > 0)
            {
                var objLast = lstBitacora.Last().Bloque;
                if (objLast != null && objLast.lstWarnings.Count > 0)
                {
                    int MaxWarnings = objLast.lstWarnings.Max(x => x.Valor);
                    var warningDict = objLast.lstWarnings.ToDictionary(x => x.Bloque.Id, x => x.Valor);
                    foreach (var obj in lst)
                    {
                        int ObjWarnings = warningDict.ContainsKey(obj.Id) ? warningDict[obj.Id] : 0;
                        obj.Peso = Math.Max(1, MaxWarnings - ObjWarnings);
                    }
                }
            }
            int SumaPesos = 0;
            foreach (var obj in lst)
            {
                SumaPesos += obj.Peso;
            }
            int NumeroElegido = rnd.Next(SumaPesos);
            int acumulado = 0;
            for (int l = 0; l < lst.Count; l++)
            {
                acumulado += lst[l].Peso;
                if (NumeroElegido < acumulado)
                {
                    return lst[l];
                }
            }
            return lst[lst.Count - 1];
        }
        #endregion

        #region HashSudoku
        private string GenerarHashSudoku()
        {
            if (lstCeldas == null || lstCeldas.Count == 0) return string.Empty;
            var valores = lstCeldas
                .OrderBy(c => c.CuadranteEjeY)
                .ThenBy(c => c.EjeY)
                .ThenBy(c => c.CuadranteEjeX)
                .ThenBy(c => c.EjeX)
                .Select(c => c.Valor.ToString())
                .ToArray();
            return string.Join("", valores);
        }
        #endregion

        #region Mapeo Valores
        private void InicializarMapeoValores()
        {
            mapeoValores = new Dictionary<int, int>();
            var valoresDisponibles = Enumerable.Range(ValorInicial, ValorFinal).ToList();
            for (int i = valoresDisponibles.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                int temp = valoresDisponibles[i];
                valoresDisponibles[i] = valoresDisponibles[j];
                valoresDisponibles[j] = temp;
            }
            for (int i = 0; i < valoresDisponibles.Count; i++)
            {
                mapeoValores[i + ValorInicial] = valoresDisponibles[i];
            }
            lstCeldas.Select(c => { c.Valor = MapearValor(c.Valor); return c; }).ToList();
        }
        private int MapearValor(int valorSecuencial)
        {
            return mapeoValores.ContainsKey(valorSecuencial) ? mapeoValores[valorSecuencial] : valorSecuencial;
        }
        #endregion

        #region Celda
        [Serializable]
        public class Celda
        {
            public int Id;
            public int IdCuadrante;
            public int CuadranteEjeX;
            public int CuadranteEjeY;
            public int EjeX;
            public int EjeY;
            public int Peso { get; set; } = 1;
            public int Valor;
            public bool bloqueado;
            public Celda() { }
            public Celda(int Id, int IdCuadrante, int CuadranteEjeX, int CuadranteEjeY, int EjeX, int EjeY)
            {
                this.Id = Id;
                this.IdCuadrante = IdCuadrante;
                this.CuadranteEjeX = CuadranteEjeX;
                this.CuadranteEjeY = CuadranteEjeY;
                this.EjeX = EjeX;
                this.EjeY = EjeY;
                lstWarnings = new List<Bitacora>();
            }
            public List<Bitacora> lstWarnings;
        }
        #endregion

        #region Bitacora
        public class Bitacora
        {
            public Celda Bloque { get; set; }
            public int Valor { get; set; }
        }
        #endregion
    }

    #region Tipo
    public enum eType
    {
        cr9x9, cr16x16, cr6x6, cr4x4, cr20x20, cr25x25, cr30x30, cr36x36
    }
    #endregion

    #region Alphabet
    public class Alphabet
    {
        public const string masterAlpha = "-123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ0";
        public static string getAlphaChar(int index)
        {
            return ((index > masterAlpha.Count()) ? masterAlpha[0] : masterAlpha[index]).ToString();
        }
        public static string getCurrentAlpha(int length)
        {
            if (length < 0) return string.Empty;
            if (length >= masterAlpha.Length) return masterAlpha;
            return masterAlpha.Substring(0, length);
        }
        public static string getAlphaChar(int length, int index)
        {
            return (index < 0 || index > length ? masterAlpha[0] : masterAlpha[index]).ToString();
        }
        public static string getAlphaChar(int length, char caracter)
        {
            return getAlphaChar(length, getAlphaIndex(length, caracter));
        }
        public static int getAlphaIndex(int length, char caracter)
        {
            int index = masterAlpha.IndexOf(caracter);
            return index < 0 || index > length ? 0 : index;
        }
        public static int getAlphaIndex(char caracter)
        {
            int index = masterAlpha.IndexOf(caracter);
            return index < 0 ? 0 : index;
        }
    }
    #endregion
}