using System;
using System.Linq;
using System.Linq.Expressions;
using SPMeta2.Utils;

namespace SPMeta2.Containers.Assertion
{
    public class AssertPair<TSrc, TDst>
    {
        #region constructors

        public AssertPair(TSrc src, TDst dst)
        {
            Src = src;
            Dst = dst;
        }

        #endregion

        #region properties

        public object Tag { get; set; }

        public TSrc Src { get; set; }
        public TDst Dst { get; set; }

        #endregion

        #region events

        public EventHandler<OnPropertyValidatedEventArgs> OnPropertyValidated;

        protected void InvokeOnPropertyValidated(object sender, OnPropertyValidatedEventArgs args)
        {
            if (OnPropertyValidated != null)
                OnPropertyValidated(sender, args);
        }

        #endregion

        #region internal

        protected virtual AssertPair<TSrc, TDst> InternalShouldBeEqual<TType>(Expression<Func<TSrc, TType>> srcExp, Expression<Func<TSrc, TType>> srcAlias, Expression<Func<TDst, TType>> dstExp, Expression<Func<TDst, TType>> dstAlias)
        {
            var srcProp = Src.GetExpressionValue<TSrc, TType>(srcExp);
            var dstProp = Dst.GetExpressionValue<TDst, TType>(dstExp);

            var srcPropAlias = Src.GetExpressionValue<TSrc, TType>(srcAlias);
            var dstPropAlias = Dst.GetExpressionValue<TDst, TType>(dstAlias);

            var isValid = object.Equals(srcProp.Value, dstProp.Value);

            if (srcProp.Value is byte[] && dstProp.Value is byte[])
                isValid = (srcProp.Value as byte[]).SequenceEqual((dstProp.Value as byte[]));

            if (srcProp.Value is DateTime && dstProp.Value is DateTime)
                isValid = DateTime.Equals(srcProp.Value, dstProp.Value);

            InvokeOnPropertyValidated(this, new OnPropertyValidatedEventArgs
            {
                Result = new PropertyValidationResult
                {
                    Tag = this.Tag,
                    Src = srcPropAlias,
                    Dst = dstPropAlias,
                    IsValid = isValid
                }
            });

            return this;
        }

        #endregion

        #region should be equal

        public AssertPair<TSrc, TDst> ShouldBeEqual(Func<AssertPair<TSrc, TDst>, TSrc, TDst, PropertyValidationResult> result)
        {
            InvokeOnPropertyValidated(this, new OnPropertyValidatedEventArgs
            {
                Result = result(this, this.Src, this.Dst)
            });

            return this;
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, DateTime>> srcExp, Expression<Func<TDst, DateTime>> dstExp)
        {
            return InternalShouldBeEqual<DateTime>(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, double?>> srcExp, Expression<Func<TDst, double?>> dstExp)
        {
            return InternalShouldBeEqual<double?>(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, double>> srcExp, Expression<Func<TDst, double>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, Guid?>> srcExp, Expression<Func<TDst, Guid?>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, bool?>> srcExp, Expression<Func<TDst, bool?>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, string>> srcExp, Expression<Func<TDst, string>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, string>> srcExp, Expression<Func<TSrc, string>> srcAlias, Expression<Func<TDst, string>> dstExp, Expression<Func<TDst, string>> dstAlias)
        {
            return InternalShouldBeEqual<string>(srcExp, srcAlias, dstExp, dstAlias);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, uint>> srcExp, Expression<Func<TDst, uint>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, Guid?>> srcExp, Expression<Func<TSrc, Guid?>> srcAlias, Expression<Func<TDst, Guid?>> dstExp, Expression<Func<TDst, Guid?>> dstAlias)
        {
            return InternalShouldBeEqual<Guid?>(srcExp, srcAlias, dstExp, dstAlias);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, uint>> srcExp, Expression<Func<TSrc, uint>> srcAlias, Expression<Func<TDst, uint>> dstExp, Expression<Func<TDst, uint>> dstAlias)
        {
            return InternalShouldBeEqual<uint>(srcExp, srcAlias, dstExp, dstAlias);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, double>> srcExp, Expression<Func<TSrc, double>> srcAlias, Expression<Func<TDst, double>> dstExp, Expression<Func<TDst, double>> dstAlias)
        {
            return InternalShouldBeEqual<double>(srcExp, srcAlias, dstExp, dstAlias);
        }


        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, byte[]>> srcExp, Expression<Func<TDst, byte[]>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, byte[]>> srcExp, Expression<Func<TSrc, byte[]>> srcAlias, Expression<Func<TDst, byte[]>> dstExp, Expression<Func<TDst, byte[]>> dstAlias)
        {
            return InternalShouldBeEqual<byte[]>(srcExp, srcAlias, dstExp, dstAlias);
        }


        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, Guid>> srcExp, Expression<Func<TDst, Guid>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, Guid>> srcExp, Expression<Func<TSrc, Guid>> srcAlias, Expression<Func<TDst, Guid>> dstExp, Expression<Func<TDst, Guid>> dstAlias)
        {
            return InternalShouldBeEqual<Guid>(srcExp, srcAlias, dstExp, dstAlias);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, bool>> srcExp, Expression<Func<TDst, bool>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, bool?>> srcExp, Expression<Func<TSrc, bool?>> srcAlias, Expression<Func<TDst, bool?>> dstExp, Expression<Func<TDst, bool?>> dstAlias)
        {
            return InternalShouldBeEqual<bool?>(srcExp, srcAlias, dstExp, dstAlias);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, bool>> srcExp, Expression<Func<TSrc, bool>> srcAlias, Expression<Func<TDst, bool>> dstExp, Expression<Func<TDst, bool>> dstAlias)
        {
            return InternalShouldBeEqual<bool>(srcExp, srcAlias, dstExp, dstAlias);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, int?>> srcExp, Expression<Func<TDst, int?>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, int?>> srcExp, Expression<Func<TSrc, int?>> srcAlias, Expression<Func<TDst, int?>> dstExp, Expression<Func<TDst, int?>> dstAlias)
        {
            return InternalShouldBeEqual<int?>(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, int>> srcExp, Expression<Func<TDst, int>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, short>> srcExp, Expression<Func<TDst, short>> dstExp)
        {
            return ShouldBeEqual(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, short>> srcExp, Expression<Func<TSrc, short>> srcAlias,
            Expression<Func<TDst, short>> dstExp, Expression<Func<TDst, short>> dstAlias)
        {
            return InternalShouldBeEqual<short>(srcExp, srcAlias, dstExp, dstAlias);
        }

        public AssertPair<TSrc, TDst> ShouldBeEqual(Expression<Func<TSrc, int>> srcExp, Expression<Func<TSrc, int>> srcAlias, Expression<Func<TDst, int>> dstExp, Expression<Func<TDst, int>> dstAlias)
        {
            return InternalShouldBeEqual<int>(srcExp, srcExp, dstExp, dstExp);
        }

        #endregion

        #region should be part of

        public AssertPair<TSrc, TDst> ShouldBePartOf(Expression<Func<TSrc, string>> srcExp, Expression<Func<TDst, string>> dstExp)
        {
            return ShouldBePartOf(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBePartOf(Expression<Func<TSrc, string>> srcExp, Expression<Func<TSrc, string>> srcAlias, Expression<Func<TDst, string>> dstExp, Expression<Func<TDst, string>> dstAlias)
        {
            var srcProp = Src.GetExpressionValue<TSrc, string>(srcExp);
            var dstProp = Dst.GetExpressionValue<TDst, string>(dstExp);

            var srcPropAlias = Src.GetExpressionValue<TSrc, string>(srcAlias);
            var dstPropAlias = Dst.GetExpressionValue<TDst, string>(dstAlias);

            InvokeOnPropertyValidated(this, new OnPropertyValidatedEventArgs
            {
                Result = new PropertyValidationResult
                {
                    Tag = this.Tag,
                    Src = srcPropAlias,
                    Dst = dstPropAlias,
                    IsValid = dstProp.Value != null &&
                              srcProp.Value != null &&
                              dstProp.Value.ToString().Contains(srcProp.Value.ToString())
                }
            });

            return this;
        }

        #endregion


        #region should be start of

        public AssertPair<TSrc, TDst> ShouldBeStartOf(Expression<Func<TSrc, string>> srcExp, Expression<Func<TDst, string>> dstExp)
        {
            return ShouldBeStartOf(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeStartOf(Expression<Func<TSrc, string>> srcExp, Expression<Func<TSrc, string>> srcAlias, Expression<Func<TDst, string>> dstExp, Expression<Func<TDst, string>> dstAlias)
        {
            var srcProp = Src.GetExpressionValue<TSrc, string>(srcExp);
            var dstProp = Dst.GetExpressionValue<TDst, string>(dstExp);

            var srcPropAlias = Src.GetExpressionValue<TSrc, string>(srcAlias);
            var dstPropAlias = Dst.GetExpressionValue<TDst, string>(dstAlias);

            InvokeOnPropertyValidated(this, new OnPropertyValidatedEventArgs
            {
                Result = new PropertyValidationResult
                {
                    Tag = this.Tag,
                    Src = srcPropAlias,
                    Dst = dstPropAlias,
                    IsValid = dstProp.Value.ToString().StartsWith(srcProp.Value.ToString())
                }
            });

            return this;
        }

        #endregion

        #region should be end of

        public AssertPair<TSrc, TDst> ShouldBeEndOf(Expression<Func<TSrc, string>> srcExp, Expression<Func<TDst, string>> dstExp)
        {
            return ShouldBeEndOf(srcExp, srcExp, dstExp, dstExp);
        }

        public AssertPair<TSrc, TDst> ShouldBeEndOf(Expression<Func<TSrc, string>> srcExp, Expression<Func<TSrc, string>> srcAlias, Expression<Func<TDst, string>> dstExp, Expression<Func<TDst, string>> dstAlias)
        {
            var srcProp = Src.GetExpressionValue<TSrc, string>(srcExp);
            var dstProp = Dst.GetExpressionValue<TDst, string>(dstExp);

            var srcPropAlias = Src.GetExpressionValue<TSrc, string>(srcAlias);
            var dstPropAlias = Dst.GetExpressionValue<TDst, string>(dstAlias);

            InvokeOnPropertyValidated(this, new OnPropertyValidatedEventArgs
            {
                Result = new PropertyValidationResult
                {
                    Tag = this.Tag,
                    Src = srcPropAlias,
                    Dst = dstPropAlias,
                    IsValid = dstProp.Value.ToString().EndsWith(srcProp.Value.ToString())
                }
            });

            return this;
        }

        #endregion

        #region  not null

        public AssertPair<TSrc, TDst> ShouldNotBeNull(object value)
        {
            InvokeOnPropertyValidated(this, new OnPropertyValidatedEventArgs
            {
                Result = new PropertyValidationResult
                {
                    Tag = this.Tag,
                    Src = null,
                    Dst = null,
                    IsValid = value != null,
                    Message = string.Format("Not null instance. Object type is:[{0}]", value.GetType().ToString())
                }
            });

            return this;
        }

        public AssertPair<TSrc, TDst> ShouldBeNull(object value)
        {
            InvokeOnPropertyValidated(this, new OnPropertyValidatedEventArgs
            {
                Result = new PropertyValidationResult
                {
                    Tag = this.Tag,
                    Src = null,
                    Dst = null,
                    IsValid = value == null,
                    Message = string.Format("Null instance")
                }
            });

            return this;
        }

        #endregion

        #region skip

        protected virtual AssertPair<TSrc, TDst> InternalSkipProperty<TType>(Expression<Func<TSrc, TType>> srcPropExp, string message)
        {
            var srcProp = Src.GetExpressionValue<TSrc, TType>(srcPropExp);

            InvokeOnPropertyValidated(this, new OnPropertyValidatedEventArgs
            {
                Result = new PropertyValidationResult
                {
                    Tag = this.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = true,
                    Message = message
                }
            });

            return this;
        }

        public AssertPair<TSrc, TDst> SkipProperty<TProp>(Expression<Func<TSrc, TProp>> srcPropExp, string message)
        {
            return InternalSkipProperty<TProp>(srcPropExp, message);
        }

        public AssertPair<TSrc, TDst> SkipProperty(Expression<Func<TSrc, string>> srcPropExp)
        {
            return SkipProperty(srcPropExp, "Property is null or empty.");
        }

        public AssertPair<TSrc, TDst> SkipProperty(Expression<Func<TSrc, string>> srcPropExp, string message)
        {
            return InternalSkipProperty<string>(srcPropExp, message);
        }

        public AssertPair<TSrc, TDst> SkipProperty(Expression<Func<TSrc, int>> srcPropExp, string message)
        {
            return InternalSkipProperty<int>(srcPropExp, message);
        }

        public AssertPair<TSrc, TDst> SkipProperty(Expression<Func<TSrc, bool>> srcPropExp, string message)
        {
            return InternalSkipProperty<bool>(srcPropExp, message);
        }

        public AssertPair<TSrc, TDst> SkipProperty(Expression<Func<TSrc, uint>> srcPropExp, string message)
        {
            return InternalSkipProperty<uint>(srcPropExp, message);
        }

        public AssertPair<TSrc, TDst> SkipProperty(Expression<Func<TSrc, Guid>> srcPropExp)
        {
            return SkipProperty(srcPropExp, "Property is null or empty.");
        }

        public AssertPair<TSrc, TDst> SkipProperty(Expression<Func<TSrc, Guid>> srcPropExp, string message)
        {
            return InternalSkipProperty<Guid>(srcPropExp, message);
        }

        #endregion

    }
}
