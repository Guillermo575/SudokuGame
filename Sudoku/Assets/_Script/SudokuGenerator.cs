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
        public bool Exito { private set; get; }
        public bool Validado { private set; get; }
        public int ConteoErrores { private set; get; }
        public long TiempoEjecutado { private set; get; }
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
                    for (int y = 0; y < ColumnasY; y++)
                    {
                        Tabla += "<tr>";
                        for (int l = 0; l < ColumnasX; l++)
                        {
                            for (int x = 0; x < ColumnasX; x++)
                            {
                                var objCelda = (from obj in lstCeldas where obj.CuadranteEjeX == l && obj.CuadranteEjeY == m && obj.EjeX == x && obj.EjeY == y select obj).ToList().First();
                                Tabla += $"<td {(objCelda.IdCuadrante % 2 == 0 ? "style='background-color:#DDD'" : "style='background-color:#FFF'")}> {Alphabet.getAlphaChar(objCelda.Valor)} </td>";
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

        #region	Test
        public static void Testeo(int cantidad = 50000)
        {
            List<SudokuGenerator> lst = new List<SudokuGenerator>();
            int ValorMinimo = 0;
            int ValorMaximo = 0;
            double ValorPromed = 0;
            double ValorPromedReit = 0;
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
            }
            stopwatch.Stop();
            TiempoEjecutado = stopwatch.ElapsedMilliseconds;
        }
        public static bool ValidarCeldas(List<Celda> lstCeldas)
        {
            bool Validado = true;
            foreach (var objCelda in lstCeldas)
            {
                if (!Validado || objCelda.Valor == 0) return false;
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
                }
                ValorActual++;
            }
            Exito = true;
            var xx = ResumenHTML;
        }
        public Celda GetCeldaElegida(List<Celda> lst)
        {
            if (lstBitacora.Count > 0)
            {
                var objLast = lstBitacora.Last().Bloque;
                if (objLast != null && objLast.lstWarnings.Count > 0)
                {
                    int MaxWarnings = objLast.lstWarnings.Max(x => x.Valor);
                    int MaxCalif = MaxWarnings;
                    foreach (var obj in lst)
                    {
                        var lstObjWarning = (from x in objLast.lstWarnings where x.Bloque.Id == obj.Id select x).ToList();
                        int ObjWarnings = lstObjWarning.Count > 0 ? lstObjWarning.Last().Valor : 0;
                        obj.Peso = ((MaxWarnings - ObjWarnings) * MaxCalif) / MaxCalif;
                        obj.Peso = obj.Peso == 0 ? 1 : obj.Peso;
                    }
                }
            }
            Random rnd = new Random();
            var SumaPesos = (from x in lst select x.Peso).Sum();
            var NumeroElegido = rnd.Next(SumaPesos);
            int IndexMinimo = 0;
            int IndexMaximo = lst[0].Peso;
            for (int l = 0; l < lst.Count; l++)
            {
                if (NumeroElegido >= IndexMinimo && NumeroElegido <= IndexMaximo)
                {
                    return lst[l];
                }
                else if (l + 1 < lst.Count)
                {
                    IndexMinimo = IndexMaximo + 1;
                    IndexMaximo = IndexMinimo + lst[l + 1].Peso;
                }
            }
            return null;
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
            }
            public List<Bitacora> lstWarnings = new List<Bitacora>();
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
    public enum eType
    {
        cr9x9, cr16x16, cr6x6, cr4x4, cr20x20, cr25x25, cr30x30, cr36x36
    }
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