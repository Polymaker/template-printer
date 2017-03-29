using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplatePrinter
{
    public struct Measure
    {
        private double value;
        private const double CmToInch = 0.393701d;
        public static readonly Measure Zero;

        public double this[UnitOfMeasure unit]
        {
            get
            {
                switch (unit) {
                    default:
                    case UnitOfMeasure.Cm:
                        return value;
                    case UnitOfMeasure.Mm:
                        return value * 10d;
                    case UnitOfMeasure.Inch:
                        return value * CmToInch;
                    case UnitOfMeasure.Foot:
                        return (value * CmToInch) / 12d;
                    case UnitOfMeasure.HundredInch:
                        return value * CmToInch * 100d;
                }
            }
        }

        public double Cm
        {
            get { return this[UnitOfMeasure.Cm]; }
        }
        
        public double Pixels(double dpi)
        {
            return this[UnitOfMeasure.Inch] * dpi;
        }

        public static Measure FromCm(double value)
        {
            return new Measure { value = value };
        }

        public static Measure FromMm(double value)
        {
            return new Measure { value = value / 10d };
        }

        public static Measure FromInch(double value)
        {
            return new Measure { value = value / CmToInch };
        }

        public static Measure FromPixels(double value, double dpi)
        {
            return new Measure { value = (value / dpi) / CmToInch };
        }

        public static Measure FromUnitMeasure(UnitOfMeasure unit, double value)
        {
            switch (unit)
            {
                default:
                case UnitOfMeasure.Cm:
                    return new Measure { value = value };
                case UnitOfMeasure.Mm:
                    return new Measure { value = value / 10d };
                case UnitOfMeasure.Inch:
                    return new Measure { value = value / CmToInch };
                case UnitOfMeasure.Foot:
                    return new Measure { value = value / CmToInch / 12d };
                case UnitOfMeasure.HundredInch:
                    return new Measure { value = value / CmToInch / 100d };
            }
        }

        public static Measure operator +(Measure m1, Measure m2)
        {
            return new Measure { value = m1.value + m2.value };
        }

        public static Measure operator -(Measure m1, Measure m2)
        {
            return new Measure { value = m1.value - m2.value };
        }

        public static Measure operator *(Measure m1, Measure m2)
        {
            return new Measure { value = m1.value * m2.value };
        }

        public static Measure operator /(Measure m1, Measure m2)
        {
            return new Measure { value = m1.value / m2.value };
        }

        public static Measure operator *(Measure m1, double m2)
        {
            return new Measure { value = m1.value * m2 };
        }

        public static Measure operator /(Measure m1, double m2)
        {
            return new Measure { value = m1.value / m2 };
        }

        #region Comparators

        public override bool Equals(object obj)
        {
            if (!(obj is Measure))
                return false;
            return value == ((Measure)obj).value;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Measure m1, Measure m2)
        {
            return m1.value == m2.value;
        }

        public static bool operator !=(Measure m1, Measure m2)
        {
            return !(m1 == m2);
        }

        public static bool operator >(Measure m1, Measure m2)
        {
            return m1.value > m2.value;
        }

        public static bool operator <(Measure m1, Measure m2)
        {
            return m1.value < m2.value;
        }

        public static bool operator >=(Measure m1, Measure m2)
        {
            return m1.value >= m2.value;
        }

        public static bool operator <=(Measure m1, Measure m2)
        {
            return m1.value <= m2.value;
        }

        #endregion
    }
}
