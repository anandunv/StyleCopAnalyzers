﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Lightup
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal struct ConstantPatternSyntaxWrapper : ISyntaxWrapper<CSharpSyntaxNode>
    {
        private const string ConstantPatternSyntaxTypeName = "Microsoft.CodeAnalysis.CSharp.Syntax.ConstantPatternSyntax";
        private static readonly Type ConstantPatternSyntaxType;

        private static readonly Func<CSharpSyntaxNode, ExpressionSyntax> ExpressionAccessor;
        private static readonly Func<CSharpSyntaxNode, ExpressionSyntax, CSharpSyntaxNode> WithExpressionAccessor;

        private readonly CSharpSyntaxNode node;

        static ConstantPatternSyntaxWrapper()
        {
            ConstantPatternSyntaxType = typeof(CSharpSyntaxNode).GetTypeInfo().Assembly.GetType(ConstantPatternSyntaxTypeName);
            ExpressionAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<CSharpSyntaxNode, ExpressionSyntax>(ConstantPatternSyntaxType, nameof(Expression));
            WithExpressionAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<CSharpSyntaxNode, ExpressionSyntax>(ConstantPatternSyntaxType, nameof(Expression));
        }

        private ConstantPatternSyntaxWrapper(CSharpSyntaxNode node)
        {
            this.node = node;
        }

        public CSharpSyntaxNode SyntaxNode => this.node;

        public ExpressionSyntax Expression
        {
            get
            {
                return ExpressionAccessor(this.SyntaxNode);
            }
        }

        public static explicit operator ConstantPatternSyntaxWrapper(PatternSyntaxWrapper node)
        {
            return (ConstantPatternSyntaxWrapper)node.SyntaxNode;
        }

        public static explicit operator ConstantPatternSyntaxWrapper(SyntaxNode node)
        {
            if (node == null)
            {
                return default(ConstantPatternSyntaxWrapper);
            }

            if (!IsInstance(node))
            {
                throw new InvalidCastException($"Cannot cast '{node.GetType().FullName}' to '{ConstantPatternSyntaxTypeName}'");
            }

            return new ConstantPatternSyntaxWrapper((CSharpSyntaxNode)node);
        }

        public static implicit operator PatternSyntaxWrapper(ConstantPatternSyntaxWrapper wrapper)
        {
            return PatternSyntaxWrapper.FromUpcast(wrapper.node);
        }

        public static implicit operator CSharpSyntaxNode(ConstantPatternSyntaxWrapper wrapper)
        {
            return wrapper.node;
        }

        public static bool IsInstance(SyntaxNode node)
        {
            return node != null && LightupHelpers.CanWrapNode(node, ConstantPatternSyntaxType);
        }

        public ConstantPatternSyntaxWrapper WithExpression(ExpressionSyntax expression)
        {
            return new ConstantPatternSyntaxWrapper(WithExpressionAccessor(this.SyntaxNode, expression));
        }
    }
}
