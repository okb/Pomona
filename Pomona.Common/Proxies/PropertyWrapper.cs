﻿#region License

// ----------------------------------------------------------------------------
// Pomona source code
// 
// Copyright © 2012 Karsten Nikolai Strand
// 
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// ----------------------------------------------------------------------------

#endregion

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Pomona.Common.Internals;

namespace Pomona.Common.Proxies
{
    public class PropertyWrapper<TOwner, TPropType>
    {
        private readonly Func<TOwner, TPropType> getter;
        private readonly Expression<Func<TOwner, TPropType>> getterExpression;
        private readonly PropertyInfo propertyInfo;
        private readonly Action<TOwner, TPropType> setter;
        private readonly Expression<Action<TOwner, TPropType>> setterExpression;


        public PropertyWrapper(string propertyName)
        {
            var ownerType = typeof (TOwner);

            propertyInfo = TypeUtils.AllBaseTypesAndInterfaces(ownerType).Select(
                t => t.GetProperty(
                    propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)).FirstOrDefault(
                        t => t != null);

            var getterSelfParam = Expression.Parameter(ownerType, "x");
            getterExpression =
                Expression.Lambda<Func<TOwner, TPropType>>(
                    Expression.MakeMemberAccess(getterSelfParam, propertyInfo), getterSelfParam);
            var setterSelfParam = Expression.Parameter(ownerType, "x");
            var setterValueParam = Expression.Parameter(typeof (TPropType), "value");

            getter = getterExpression.Compile();

            setterExpression =
                Expression.Lambda<Action<TOwner, TPropType>>(
                    Expression.Assign(Expression.Property(setterSelfParam, propertyInfo), setterValueParam),
                    setterSelfParam,
                    setterValueParam);

            setter = setterExpression.Compile();
        }


        public Func<TOwner, TPropType> Getter
        {
            get { return getter; }
        }

        public Expression<Func<TOwner, TPropType>> GetterExpression
        {
            get { return getterExpression; }
        }

        public string Name
        {
            get { return propertyInfo.Name; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
        }

        public Action<TOwner, TPropType> Setter
        {
            get { return setter; }
        }

        public Expression<Action<TOwner, TPropType>> SetterExpression
        {
            get { return setterExpression; }
        }


        public TPropType Get(TOwner obj)
        {
            return Getter(obj);
        }


        public void Set(TOwner obj, TPropType value)
        {
            Setter(obj, value);
        }
    }
}