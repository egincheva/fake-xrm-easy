﻿using System;
using System.Linq;

using Xunit;
using FakeItEasy;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk.Query;

using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Crm;
using System.Reflection;

namespace FakeXrmEasy.Tests.FakeContextTests.TranslateQueryExpressionTests
{
    public class ConditionExpressionTests
    {
        [Fact]
        public void When_executing_a_query_expression_with_a_not_implemented_operator_pull_request_exception_is_thrown()
        {
            var context = new XrmFakedContext();
            var contact1 = new Entity("contact") { Id = Guid.NewGuid() }; contact1["fullname"] = "Contact 1"; contact1["firstname"] = "First 1";
            var contact2 = new Entity("contact") { Id = Guid.NewGuid() }; contact2["fullname"] = "Contact 2"; contact2["firstname"] = "First 2";

            context.Initialize(new List<Entity>() { contact1, contact2 });

            var qe = new QueryExpression() { EntityName = "contact" };
            qe.ColumnSet = new ColumnSet(true);
            qe.Criteria = new FilterExpression(LogicalOperator.And);
            var condition = new ConditionExpression("fullname", ConditionOperator.LastXFiscalPeriods, "Contact 1");
            qe.Criteria.AddCondition(condition);

            Assert.Throws<PullRequestException>(() => XrmFakedContext.TranslateQueryExpressionToLinq(context, qe).ToList());
        }

        [Fact]
        public void When_executing_a_query_expression_with_equals_operator_right_result_is_returned()
        {
            var context = new XrmFakedContext();
            var contact1 = new Entity("contact") { Id = Guid.NewGuid() }; contact1["fullname"] = "Contact 1"; contact1["firstname"] = "First 1";
            var contact2 = new Entity("contact") { Id = Guid.NewGuid() }; contact2["fullname"] = "Contact 2"; contact2["firstname"] = "First 2";
            
            context.Initialize(new List<Entity>() {  contact1, contact2 });

            var qe = new QueryExpression() { EntityName = "contact" };
            qe.ColumnSet = new ColumnSet(true);
            qe.Criteria = new FilterExpression(LogicalOperator.And);
            var condition = new ConditionExpression("fullname", ConditionOperator.Equal, "Contact 1");
            qe.Criteria.AddCondition(condition);
            
            var result = XrmFakedContext.TranslateQueryExpressionToLinq(context, qe).ToList();

            Assert.True(result.Count() == 1);
        }

        [Fact]
        public void When_executing_a_query_expression_with_endswith_operator_right_result_is_returned()
        {
            var context = new XrmFakedContext();
            var contact1 = new Entity("contact") { Id = Guid.NewGuid() }; contact1["fullname"] = "Contact 1"; contact1["firstname"] = "First 1";
            var contact2 = new Entity("contact") { Id = Guid.NewGuid() }; contact2["fullname"] = "Contact 2"; contact2["firstname"] = "First 2";

            context.Initialize(new List<Entity>() { contact1, contact2 });

            var qe = new QueryExpression() { EntityName = "contact" };
            qe.ColumnSet = new ColumnSet(true);
            qe.Criteria = new FilterExpression(LogicalOperator.And);
            var condition = new ConditionExpression("fullname", ConditionOperator.EndsWith, "2");
            qe.Criteria.AddCondition(condition);

            var result = XrmFakedContext.TranslateQueryExpressionToLinq(context, qe).ToList();

            Assert.True(result.Count() == 1);
        }
        [Fact]
        public void When_executing_a_query_expression_with_beginswith_operator_right_result_is_returned()
        {
            var context = new XrmFakedContext();
            var contact1 = new Entity("contact") { Id = Guid.NewGuid() }; contact1["fullname"] = "1 Contact"; contact1["firstname"] = "First 1";
            var contact2 = new Entity("contact") { Id = Guid.NewGuid() }; contact2["fullname"] = "2 Contact"; contact2["firstname"] = "First 2";
            
            context.Initialize(new List<Entity>() { contact1, contact2 });

            var qe = new QueryExpression() { EntityName = "contact" };
            qe.ColumnSet = new ColumnSet(true);
            qe.Criteria = new FilterExpression(LogicalOperator.And);
            var condition = new ConditionExpression("fullname", ConditionOperator.BeginsWith, "2");
            qe.Criteria.AddCondition(condition);

            var result = XrmFakedContext.TranslateQueryExpressionToLinq(context, qe).ToList();

            Assert.True(result.Count() == 1);
        }
    }
}