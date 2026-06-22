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
        public bool Exito { private set; get; }
        public bool Validado { private set; get; }
        public int ConteoErrores { private set; get; }
        public int ConteoAciertos { private set; get; }
        public long TiempoEjecutado { private set; get; }
        private int MaxRetrocesos = 100000;
        private int ContadorRetrocesos = 0;
        private int UltimasPosiciones = 0;
        private const int UMBRAL_RESET = 50;
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
            MaxRetrocesos = ColumnasX * ColumnasY * 10000;
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
                ContadorRetrocesos = 0;
                bool resetNecesario = false;
                int CuadranteIndex = 1;
                while (CuadranteIndex <= SumaCuadrantes)
                {
                    if (ContadorRetrocesos > MaxRetrocesos)
                    {
                        resetNecesario = true;
                        break;
                    }
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
                        UltimasPosiciones = 0;
                    }
                    else
                    {
                        ConteoErrores++;
                        ContadorRetrocesos++;
                        UltimasPosiciones++;
                        if (UltimasPosiciones > UMBRAL_RESET && ValorActual > ValorInicial)
                        {
                            int retrocesosProfundos = Math.Min(SumaCuadrantes, UltimasPosiciones / 10);
                            for (int i = 0; i < retrocesosProfundos && lstBitacora.Count > 0; i++)
                            {
                                var objRemover = lstBitacora.Last();
                                objRemover.Bloque.Valor = 0;
                                lstBitacora.RemoveAt(lstBitacora.Count - 1);
                                celdaGenerada--;
                            }
                            UltimasPosiciones = 0;
                        }
                        if (lstBitacora.Count == 0)
                        {
                            resetNecesario = true;
                            break;
                        }
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
                            resetNecesario = true;
                            break;
                        }
                        lstBitacoraBloqueo.Add(new Bitacora() { Bloque = objLast.Bloque, Valor = ValorActual });
                    }
                    ConteoAciertos += celdaGenerada > ConteoAciertos ? 1 : 0;
                }
                if (resetNecesario)
                {
                    foreach (var celda in lstCeldas)
                    {
                        celda.Valor = 0;
                        celda.lstWarnings.Clear();
                    }
                    lstBitacora.Clear();
                    lstBitacoraBloqueo.Clear();
                    ValorActual = ValorInicial;
                    CuadranteIndex = 1;
                    celdaGenerada = 0;
                    ContadorRetrocesos = 0;
                    UltimasPosiciones = 0;
                }
                else
                {
                    ValorActual++;
                }
            }
            Exito = true;
        }
        public Celda GetCeldaElegida(List<Celda> lst)
        {
            int SumaPesos = 0;
            int MaxWarnings = 0;
            if (lstBitacora.Count > 0)
            {
                var objLast = lstBitacora.Last().Bloque;
                if (objLast != null && objLast.lstWarnings.Count > 0)
                {
                    MaxWarnings = objLast.lstWarnings.Max(x => x.Valor);
                    var warningDict = objLast.lstWarnings.ToDictionary(x => x.Bloque.Id, x => x.Valor);
                    foreach (var obj in lst)
                    {
                        int ObjWarnings = warningDict.ContainsKey(obj.Id) ? warningDict[obj.Id] : 0;
                        obj.Peso = Math.Max(1, MaxWarnings - ObjWarnings + 1);
                    }
                }
                else
                {
                    foreach (var obj in lst)
                    {
                        obj.Peso = 1;
                    }
                }
            }
            var celdaConMenosOpciones = lst.OrderBy(c => ContarOpcionesFuturas(c)).ToList();
            int minOpciones = ContarOpcionesFuturas(celdaConMenosOpciones.First());
            var celdasOptimas = celdaConMenosOpciones.Where(c => ContarOpcionesFuturas(c) <= minOpciones + 2).ToList();
            foreach (var obj in celdasOptimas)
            {
                int opcionesFuturas = ContarOpcionesFuturas(obj);
                int pesoBase = obj.Peso;
                if (opcionesFuturas < 3)
                {
                    obj.Peso = pesoBase * 5;
                }
                else if (opcionesFuturas < 5)
                {
                    obj.Peso = pesoBase * 3;
                }
                else
                {
                    obj.Peso = pesoBase * 2;
                }
            }
            List<Celda> lstFinal = celdasOptimas.Count > 0 ? celdasOptimas : lst;
            foreach (var obj in lstFinal)
            {
                SumaPesos += obj.Peso;
            }
            int NumeroElegido = rnd.Next(SumaPesos);
            int acumulado = 0;
            for (int l = 0; l < lstFinal.Count; l++)
            {
                acumulado += lstFinal[l].Peso;
                if (NumeroElegido < acumulado)
                {
                    return lstFinal[l];
                }
            }
            return lstFinal[lstFinal.Count - 1];
        }

        private int ContarOpcionesFuturas(Celda celda)
        {
            int opciones = 0;
            for (int valor = ValorInicial; valor <= ValorFinal; valor++)
            {
                if (ValidoEjeXY(celda, valor) && !GetBloqueo(celda, valor))
                {
                    opciones++;
                }
            }
            return opciones;
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