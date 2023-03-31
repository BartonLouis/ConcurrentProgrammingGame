using System;

namespace Interpreter
{
    public static class Operations
    {
        public static IntValue Add(Value v1, Value v2)
        {
            IntValue i1 = v1.GetAsInteger();
            IntValue i2 = v2.GetAsInteger();
            try
            {
                return IntValue.Add(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static IntValue Sub(Value v1, Value v2)
        {
            IntValue i1 = v1.GetAsInteger();
            IntValue i2 = v2.GetAsInteger();
            try
            {
                return IntValue.Sub(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static IntValue Mul(Value v1, Value v2)
        {
            IntValue i1 = v1.GetAsInteger();
            IntValue i2 = v2.GetAsInteger();
            try
            {
                return IntValue.Mul(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static IntValue Div(Value v1, Value v2)
        {
            IntValue i1 = v1.GetAsInteger();
            IntValue i2 = v2.GetAsInteger();
            try
            {
                return IntValue.Div(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static IntValue Pow(Value v1, Value v2)
        {
            IntValue i1 = v1.GetAsInteger();
            IntValue i2 = v2.GetAsInteger();
            try
            {
                return IntValue.Pow(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static IntValue Mod(Value v1, Value v2)
        {
            IntValue i1 = v1.GetAsInteger();
            IntValue i2 = v2.GetAsInteger();
            try
            {
                return IntValue.Mod(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue EQ(Value v1, Value v2)
        {
            try
            {
                if (v1.GetType() == typeof(BoolValue))
                {
                    BoolValue b1 = v1.GetAsBoolean();
                    BoolValue b2 = v2.GetAsBoolean();
                    return BoolValue.EQ(b1, b2);
                }
                else if (v1.GetType() == typeof(IntValue))
                {
                    IntValue i1 = v1.GetAsInteger();
                    IntValue i2 = v2.GetAsInteger();
                    return IntValue.EQ(i1, i2);
                }
                else if (v1.GetType() == typeof(StringValue))
                {
                    StringValue s1 = v1.GetAsString();
                    StringValue s2 = v2.GetAsString();
                    return StringValue.EQ(s1, s2);
                }
                else if (v1.GetType() == typeof(SideValue))
                {
                    SideValue s1 = v1.GetAsSide();
                    SideValue s2 = v2.GetAsSide();
                    return SideValue.EQ(s1, s2);
                }
                else if (v1.GetType() == typeof(PlayerValue))
                {
                    PlayerValue p1 = v1.GetAsPlayer();
                    PlayerValue p2 = v2.GetAsPlayer();
                    return PlayerValue.EQ(p1, p2);
                }
                else if (v1.GetType() == typeof(ClassValue))
                {
                    ClassValue c1 = v1.GetAsClass();
                    ClassValue c2 = v2.GetAsClass();
                    return ClassValue.EQ(c1, c2);
                }
                else
                {
                    return null;
                }
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue NEQ(Value v1, Value v2)
        {
            try
            {
                if (v1.GetType() == typeof(BoolValue))
                {
                    BoolValue b1 = v1.GetAsBoolean();
                    BoolValue b2 = v2.GetAsBoolean();
                    return BoolValue.NEQ(b1, b2);
                }
                else if (v1.GetType() == typeof(IntValue))
                {
                    IntValue i1 = v1.GetAsInteger();
                    IntValue i2 = v2.GetAsInteger();
                    return IntValue.NEQ(i1, i2);
                }
                else if (v1.GetType() == typeof(StringValue))
                {
                    StringValue s1 = v1.GetAsString();
                    StringValue s2 = v2.GetAsString();
                    return StringValue.NEQ(s1, s2);
                }
                else if (v1.GetType() == typeof(SideValue))
                {
                    SideValue s1 = v1.GetAsSide();
                    SideValue s2 = v2.GetAsSide();
                    return SideValue.NEQ(s1, s2);
                }
                else if (v1.GetType() == typeof(PlayerValue))
                {
                    PlayerValue p1 = v1.GetAsPlayer();
                    PlayerValue p2 = v2.GetAsPlayer();
                    return PlayerValue.NEQ(p1, p2);
                }
                else if (v1.GetType() == typeof(ClassValue))
                {
                    ClassValue c1 = v1.GetAsClass();
                    ClassValue c2 = v2.GetAsClass();
                    return ClassValue.NEQ(c1, c2);
                }
                else
                {
                    return null;
                }
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue GT(Value v1, Value v2)
        {
            try
            {
                IntValue i1 = v1.GetAsInteger();
                IntValue i2 = v2.GetAsInteger();
                return IntValue.GT(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue LT(Value v1, Value v2)
        {
            try
            {
                IntValue i1 = v1.GetAsInteger();
                IntValue i2 = v2.GetAsInteger();
                return IntValue.LT(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue GTEQ(Value v1, Value v2)
        {
            try
            {
                IntValue i1 = v1.GetAsInteger();
                IntValue i2 = v2.GetAsInteger();
                return IntValue.GTEQ(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue LTEQ(Value v1, Value v2)
        {
            try
            {
                IntValue i1 = v1.GetAsInteger();
                IntValue i2 = v2.GetAsInteger();
                return IntValue.LTEQ(i1, i2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue And(Value v1, Value v2)
        {
            try
            {
                BoolValue b1 = v1.GetAsBoolean();
                BoolValue b2 = v2.GetAsBoolean();
                return BoolValue.And(b1, b2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue Or(Value v1, Value v2)
        {
            try
            {
                BoolValue b1 = v1.GetAsBoolean();
                BoolValue b2 = v2.GetAsBoolean();
                return BoolValue.Or(b1, b2);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static BoolValue Not(Value v1)
        {
            try
            {
                BoolValue b1 = v1.GetAsBoolean();
                return BoolValue.Not(b1);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
