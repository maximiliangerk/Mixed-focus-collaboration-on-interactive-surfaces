using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace team20_bp2015
{
    public class Vector3
    {
        /// <summary>
        /// Nullvector für spätere Berechnungen
        /// </summary>
        public static Vector3 nullVector = new Vector3(0, 0, 0);

        /// <summary>
        /// Konstruktor für leere Vectoren 
        /// </summary>
        public Vector3()
        {

        }
        /// <summary>
        /// Konstruktor mit Übergabe der Koordinaten
        /// </summary>
        /// <param name="_x">Koordinate X</param>
        /// <param name="_y">Koordinate Y</param>
        /// <param name="_z">Koordinate Z</param>
        public Vector3(double _x, double _y, double _z)
        {
            X = _x;

            Y = _y;

            Z = _z;
        }
        /// <summary>
        /// private X Variable, von der die public X Variable zugreift
        /// </summary>
        public double _x;
        /// <summary>
        /// get/set von X
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        /// <summary>
        /// private Y Variable, von der die public Y Variable zugreift
        /// </summary>
        public double _y;
        /// <summary>
        /// get/set von Y
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        /// <summary>
        /// private Z Variable, von der die public Z Variable zugreift
        /// </summary>
        public double _z;
        /// <summary>
        /// get/set von Z
        /// </summary>
        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }
        /// <summary>
        /// Berechnung des Winkels zwischen den EingabeVektoren
        /// </summary>
        /// <param name="v1">Eingabevektor 1</param>
        /// <param name="v2">Eingabevektor 2</param>
        /// <returns></returns>
        public double angleBetween2Vectors(Vector3 v1, Vector3 v2)
        {
            double result = 0;

            double temp = 0;

            result = ScalarProduct(v1, v2);

            temp = NormVector3(v1) * NormVector3(v2);

            result = result / temp;

            result = Math.Acos(result);

            return result;
        }
        /// <summary>
        /// Berechnung des Skalarprodukts
        /// </summary>
        /// <param name="v1">Eingabevektor 1</param>
        /// <param name="v2">Eingabevektor 2</param>
        /// <returns></returns>
        public double ScalarProduct(Vector3 v1, Vector3 v2)
        {
            double result = 0;

            result = v1.X * v2.X;

            result += v1.Y * v2.Y;

            result += v1.Z * v2.Z;

            return result;
        }
        /// <summary>
        /// Normalisierung des Eingabevektors
        /// </summary>
        /// <param name="v1">Eingabevektor</param>
        /// <returns></returns>
        public double NormVector3(Vector3 v1)
        {
            double result = 0;

            result = v1.X * v1.X;

            result += v1.Y * v1.Y;

            result += v1.Z * v1.Z;

            result = Math.Sqrt(result);

            return result;
        }
        /// <summary>
        /// Berechnung eines Vektors, der durch Vector, Distanz und Winkel gegeben ist
        /// </summary>
        /// <param name="v1">Vektor</param>
        /// <param name="distance">Distanz</param>
        /// <param name="angle">Winkel</param>
        /// <returns></returns>
        public Vector3 vectorFromPointDistanceAngle(Vector3 v1, double distance, double angle)
        {
            Vector3 result = null;

            double tempX = 0;

            double tempY = 0;

            tempX = distance * Math.Cos(angle);

            tempY = distance * Math.Sin(angle);

            result = new Vector3(tempX, tempY, 0);
            
            return result;
        }

        /// <summary>
        /// Transformation eines Vektors in ein Koordinatensystem mit gleichem Ursprung und verschiedenen Koordinatenvektoren
        /// Drehung erfolgt auf x/y-Ebene
        /// </summary>
        /// <param name="v1">der Vector, der in ein gedrehtes Koordinatensystem transformiert werden soll</param>
        /// <param name="angle">der Winkel, der der die Drehung(Transformation) gegen den Uhrzeigersinn bestimmt</param>
        /// <returns></returns>
        public Vector3 coordinateTransformationRotateXY(Vector3 v1, double angle)
        {
            Vector3 result = null;

            double xTransformed = 0;

            double yTransformed = 0;

            xTransformed = v1.X * Math.Cos(angle) + v1.Y * Math.Sin(angle);

            yTransformed = (-1) * v1.X * Math.Sin(angle) + v1.Y * Math.Cos(angle);

            result = new Vector3(xTransformed, yTransformed, v1.Z);

            return result;
        }
    }
}
