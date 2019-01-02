using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Wootrix.Data;
using WootrixV2.Models;

namespace WootrixV2.Data
{
    public class DataAccessLayer
    {
        private readonly ApplicationDbContext _context;

        public DataAccessLayer(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetCountries()
        {
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            List<SelectListItem> list = this._context.CompanyLocCountries.AsNoTracking<CompanyLocCountries>().OrderBy<CompanyLocCountries, string>((Expression<Func<CompanyLocCountries, string>>)(n => n.country_name)).Select<CompanyLocCountries, SelectListItem>(Expression.Lambda<Func<CompanyLocCountries, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), )))); //unable to render the statement
            SelectListItem selectListItem = new SelectListItem()
            {
                Value = (string)null,
                Text = "--- Select Country ---"
            };
            list.Insert(0, selectListItem);
            return (IEnumerable<SelectListItem>)new SelectList((IEnumerable)list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetNullStatesOrCities()
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Value = (string)null,
                Text = "--- Select Country ---"
            };
            selectListItemList.Insert(0, selectListItem);
            return (IEnumerable<SelectListItem>)new SelectList((IEnumerable)selectListItemList, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetStates(string countyCode)
        {
            if (string.IsNullOrWhiteSpace(countyCode))
                return (IEnumerable<SelectListItem>)null;
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            return (IEnumerable<SelectListItem>); //unable to render the statement
        }

        public IEnumerable<SelectListItem> GetCities(
          string countryCode,
          string stateCode)
        {
            if (string.IsNullOrWhiteSpace(stateCode))
                return (IEnumerable<SelectListItem>)null;
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            return (IEnumerable<SelectListItem>); //unable to render the statement
        }

        public IEnumerable<SelectListItem> GetDepartments(int companyID)
        {
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            List<SelectListItem> list = this._context.CompanyDepartments.AsNoTracking<CompanyDepartments>().Where<CompanyDepartments>((Expression<Func<CompanyDepartments, bool>>)(n => n.CompanyID == companyID)).OrderBy<CompanyDepartments, string>((Expression<Func<CompanyDepartments, string>>)(n => n.CompanyDepartmentName)).Select<CompanyDepartments, SelectListItem>(Expression.Lambda<Func<CompanyDepartments, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), (Expression)Expression.Call(n.ID, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(int.ToString)), Array.Empty<Expression>())), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Text)), (Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(CompanyDepartments.get_CompanyDepartmentName))))), parameterExpression)).ToList<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Value = (string)null,
                Text = "--- Select Department ---"
            };
            list.Insert(0, selectListItem);
            return (IEnumerable<SelectListItem>)new SelectList((IEnumerable)list, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetInterfaceLanguages(
          int companyID,
          IOptions<RequestLocalizationOptions> rlo)
        {
            return (IEnumerable<SelectListItem>)rlo.Value.get_SupportedUICultures().Select<CultureInfo, SelectListItem>((Func<CultureInfo, SelectListItem>)(c => new SelectListItem()
            {
                Value = c.DisplayName,
                Text = c.DisplayName
            })).ToList<SelectListItem>();
        }

        public IEnumerable<SelectListItem> GetGenders()
        {
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            SelectListItem selectListItem1 = new SelectListItem()
            {
                Value = "Not Identified",
                Text = "Not Identified"
            };
            SelectListItem selectListItem2 = new SelectListItem()
            {
                Value = "Male",
                Text = "Male"
            };
            SelectListItem selectListItem3 = new SelectListItem()
            {
                Value = "Female",
                Text = "Female"
            };
            SelectListItem selectListItem4 = new SelectListItem()
            {
                Value = "Other",
                Text = "Other"
            };
            selectListItemList.Insert(0, selectListItem1);
            selectListItemList.Insert(1, selectListItem2);
            selectListItemList.Insert(2, selectListItem3);
            selectListItemList.Insert(3, selectListItem4);
            return (IEnumerable<SelectListItem>)new SelectList((IEnumerable)selectListItemList, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetCompanies()
        {
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            List<SelectListItem> list = this._context.Company.AsNoTracking<Company>().OrderBy<Company, string>((Expression<Func<Company, string>>)(n => n.CompanyName)).Select<Company, SelectListItem>(Expression.Lambda<Func<Company, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), (Expression)Expression.Call(n.ID, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(int.ToString)), Array.Empty<Expression>())), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Text)), (Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(Company.get_CompanyName))))), parameterExpression)).ToList<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem()
            {
                Value = (string)null,
                Text = "--- Select Company ---"
            };
            list.Insert(0, selectListItem);
            return (IEnumerable<SelectListItem>)new SelectList((IEnumerable)list, "Value", "Text");
        }

        public List<SelectListItem> GetGroups(int companyID)
        {
            using (ApplicationDbContext context = this._context)
            {
                ParameterExpression parameterExpression;
                // ISSUE: method reference
                // ISSUE: method reference
                // ISSUE: method reference
                // ISSUE: method reference
                List<SelectListItem> list = context.CompanyGroups.AsNoTracking<CompanyGroups>().Where<CompanyGroups>((Expression<Func<CompanyGroups, bool>>)(n => n.CompanyID == companyID)).OrderBy<CompanyGroups, string>((Expression<Func<CompanyGroups, string>>)(n => n.GroupName)).Select<CompanyGroups, SelectListItem>(Expression.Lambda<Func<CompanyGroups, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), (Expression)Expression.Call(n.ID, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(int.ToString)), Array.Empty<Expression>())), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Text)), (Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(CompanyGroups.get_GroupName))))), parameterExpression)).ToList<SelectListItem>();
                SelectListItem selectListItem = new SelectListItem()
                {
                    Value = (string)null,
                    Text = "--- Select Groups ---"
                };
                list.Insert(0, selectListItem);
                return list;
            }
        }

        public List<SelectListItem> GetListGroups(int companyID)
        {
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            return this._context.CompanyGroups.AsNoTracking<CompanyGroups>().Where<CompanyGroups>((Expression<Func<CompanyGroups, bool>>)(n => n.CompanyID == companyID)).OrderBy<CompanyGroups, string>((Expression<Func<CompanyGroups, string>>)(n => n.GroupName)).Select<CompanyGroups, SelectListItem>(Expression.Lambda<Func<CompanyGroups, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), )))); //unable to render the statement
        }

        public List<SelectListItem> GetListTopics(int companyID)
        {
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            return this._context.CompanyTopics.AsNoTracking<CompanyTopics>().Where<CompanyTopics>((Expression<Func<CompanyTopics, bool>>)(n => n.CompanyID == companyID)).OrderBy<CompanyTopics, string>((Expression<Func<CompanyTopics, string>>)(n => n.Topic)).Select<CompanyTopics, SelectListItem>(Expression.Lambda<Func<CompanyTopics, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), )))); //unable to render the statement
        }

        public List<SelectListItem> GetListTypeOfUser(int companyID)
        {
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            return this._context.CompanyTypeOfUser.AsNoTracking<CompanyTypeOfUser>().Where<CompanyTypeOfUser>((Expression<Func<CompanyTypeOfUser, bool>>)(n => n.CompanyID == companyID)).OrderBy<CompanyTypeOfUser, string>((Expression<Func<CompanyTypeOfUser, string>>)(n => n.TypeOfUser)).Select<CompanyTypeOfUser, SelectListItem>(Expression.Lambda<Func<CompanyTypeOfUser, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), )))); //unable to render the statement
        }

        public List<SelectListItem> GetListLanguages(
          int companyID,
          IOptions<RequestLocalizationOptions> rlo)
        {
            return rlo.Value.get_SupportedUICultures().Select<CultureInfo, SelectListItem>((Func<CultureInfo, SelectListItem>)(c => new SelectListItem()
            {
                Value = c.DisplayName,
                Text = c.DisplayName
            })).ToList<SelectListItem>();
        }

        public List<SelectListItem> GetArticleSegments(int companyID)
        {
            ParameterExpression parameterExpression;
            // ISSUE: method reference
            // ISSUE: method reference
            return this._context.CompanySegment.AsNoTracking<CompanySegment>().Where<CompanySegment>((Expression<Func<CompanySegment, bool>>)(n => n.CompanyID == companyID)).OrderBy<CompanySegment, string>((Expression<Func<CompanySegment, string>>)(n => n.Title)).Select<CompanySegment, SelectListItem>(Expression.Lambda<Func<CompanySegment, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), )))); //unable to render the statement
        }

        public List<SegmentArticle> GetArticlesList(int companyID)
        {
            return this._context.SegmentArticle.AsNoTracking<SegmentArticle>().Where<SegmentArticle>((Expression<Func<SegmentArticle, bool>>)(n => n.CompanyID == companyID)).OrderBy<SegmentArticle, int?>((Expression<Func<SegmentArticle, int?>>)(n => n.Order)).ToList<SegmentArticle>();
        }

        public List<CompanySegment> GetSegmentsList(
          int companyID,
          User usr,
          string segmentSearchString,
          string articleSearchString)
        {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            DataAccessLayer.\u003C\u003Ec__DisplayClass17_0 cDisplayClass170 = new DataAccessLayer.\u003C\u003Ec__DisplayClass17_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass170.companyID = companyID;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass170.articleSearchString = articleSearchString;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass170.segmentSearchString = segmentSearchString;
            List<CompanySegment> source = new List<CompanySegment>();
            List<SegmentArticle> segmentArticleList = new List<SegmentArticle>();
            List<SegmentArticle> list = this._context.SegmentArticle.ToList<SegmentArticle>();
            // ISSUE: reference to a compiler-generated field
            if (!string.IsNullOrEmpty(cDisplayClass170.articleSearchString))
            {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                list = this._context.SegmentArticle.Where<SegmentArticle>((Expression<Func<SegmentArticle, bool>>)(n => n.CompanyID == cDisplayClass170.companyID)).Where<SegmentArticle>((Expression<Func<SegmentArticle, bool>>)(m => m.Title.Contains(cDisplayClass170.articleSearchString) || m.Tags.Contains(cDisplayClass170.articleSearchString))).ToList<SegmentArticle>();
            }
            foreach (SegmentArticle segmentArticle in (IEnumerable<SegmentArticle>)this._context.SegmentArticle)
            {
                if (segmentArticle.CompanyID == usr.CompanyID)
                {
                    DateTime? publishFrom = segmentArticle.PublishFrom;
                    DateTime now = DateTime.Now;
                    if (publishFrom.HasValue && publishFrom.GetValueOrDefault() < now && (segmentArticle.Country == usr.Country && segmentArticle.State == usr.State && segmentArticle.City == usr.City) && (this.PassesFilter(segmentArticle.Groups, usr.Groups) && this.PassesFilter(segmentArticle.TypeOfUser, usr.TypeOfUser) && this.PassesFilter(segmentArticle.Topics, usr.Topics) && this.PassesFilter(segmentArticle.Languages, usr.WebsiteLanguage)))
                        segmentArticleList.Add(segmentArticle);
                }
            }
            foreach (SegmentArticle segmentArticle in segmentArticleList)
            {
                foreach (string str in ((IEnumerable<string>)segmentArticle.Segments.Split('|', StringSplitOptions.None)).ToList<string>())
                {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    DataAccessLayer.\u003C\u003Ec__DisplayClass17_1 cDisplayClass171 = new DataAccessLayer.\u003C\u003Ec__DisplayClass17_1();
                    // ISSUE: reference to a compiler-generated field
                    cDisplayClass171.CS\u0024\u003C\u003E8__locals1 = cDisplayClass170;
                    // ISSUE: reference to a compiler-generated field
                    cDisplayClass171.justSegTitle = str.Split('/', StringSplitOptions.None);
                    // ISSUE: reference to a compiler-generated method
                    if (source.FirstOrDefault<CompanySegment>(new Func<CompanySegment, bool>(cDisplayClass171.\u003CGetSegmentsList\u003Eb__3)) == null)
                    {
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        if (string.IsNullOrEmpty(cDisplayClass171.CS\u0024\u003C\u003E8__locals1.segmentSearchString))
                        {
                            ParameterExpression parameterExpression;
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: method reference
                            CompanySegment companySegment = this._context.CompanySegment.FirstOrDefault<CompanySegment>(Expression.Lambda<Func<CompanySegment, bool>>((Expression)Expression.Equal(p.Title, (Expression)Expression.Call(cDisplayClass171.justSegTitle[0], (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(object.ToString)), Array.Empty<Expression>())), parameterExpression));
                            if (companySegment != null)
                                source.Add(companySegment);
                        }
                        else
                        {
                            ParameterExpression parameterExpression;
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: method reference
                            // ISSUE: method reference
                            // ISSUE: method reference
                            // ISSUE: field reference
                            // ISSUE: field reference
                            // ISSUE: method reference
                            // ISSUE: method reference
                            // ISSUE: field reference
                            // ISSUE: field reference
                            CompanySegment companySegment = this._context.CompanySegment.FirstOrDefault<CompanySegment>(Expression.Lambda<Func<CompanySegment, bool>>((Expression)Expression.AndAlso((Expression)Expression.Equal(p.Title, (Expression)Expression.Call(cDisplayClass171.justSegTitle[0], (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(object.ToString)), Array.Empty<Expression>())), (Expression)Expression.OrElse((Expression)Expression.Call((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(CompanySegment.get_Title))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.Contains)), new Expression[1]
                            {
                (Expression) Expression.Field((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass171, typeof (DataAccessLayer.\u003C\u003Ec__DisplayClass17_1)), FieldInfo.GetFieldFromHandle(__fieldref (DataAccessLayer.\u003C\u003Ec__DisplayClass17_1.CS\u0024\u003C\u003E8__locals1))), FieldInfo.GetFieldFromHandle(__fieldref (DataAccessLayer.\u003C\u003Ec__DisplayClass17_0.segmentSearchString)))
                          }), (Expression)Expression.Call((Expression)Expression.Property((Expression)parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(CompanySegment.get_Tags))), (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(string.Contains)), new Expression[1]
  {
                (Expression) Expression.Field((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass171, typeof (DataAccessLayer.\u003C\u003Ec__DisplayClass17_1)), FieldInfo.GetFieldFromHandle(__fieldref (DataAccessLayer.\u003C\u003Ec__DisplayClass17_1.CS\u0024\u003C\u003E8__locals1))), FieldInfo.GetFieldFromHandle(__fieldref (DataAccessLayer.\u003C\u003Ec__DisplayClass17_0.segmentSearchString)))
                        }))), parameterExpression));
                    if (companySegment != null)
                        source.Add(companySegment);
                }
            }
        }
    }
      return source.OrderBy<CompanySegment, int?>((Func<CompanySegment, int?>) (o => o.Order)).ToList<CompanySegment>();
    }

public bool PassesFilter(string articleCSVFilter, string userCSVFilter)
{
    bool flag = false;
    if (string.IsNullOrEmpty(articleCSVFilter) && string.IsNullOrEmpty(userCSVFilter))
        return true;
    foreach (string str in ((IEnumerable<string>)userCSVFilter.Split('|', StringSplitOptions.None)).ToList<string>())
    {
        if (articleCSVFilter != null && articleCSVFilter.Contains(str))
            return true;
    }
    return flag;
}

public List<SegmentArticleComment> GetArticleCommentsList(int ArticleID)
{
    return this._context.SegmentArticleComment.AsNoTracking<SegmentArticleComment>().Where<SegmentArticleComment>((Expression<Func<SegmentArticleComment, bool>>)(n => n.SegmentArticleID == ArticleID && n.Status == "Approved")).OrderBy<SegmentArticleComment, DateTime>((Expression<Func<SegmentArticleComment, DateTime>>)(n => n.CreatedDate)).ToList<SegmentArticleComment>();
}

public int GetArticleApprovedCommentCount(int ArticleID)
{
    return this._context.SegmentArticleComment.Where<SegmentArticleComment>((Expression<Func<SegmentArticleComment, bool>>)(m => m.SegmentArticleID == ArticleID && m.Status == "Approved")).Count<SegmentArticleComment>();
}

public int GetArticleReviewCommentCount(int CompanyID)
{
    return this._context.SegmentArticleComment.Where<SegmentArticleComment>((Expression<Func<SegmentArticleComment, bool>>)(m => m.CompanyID == CompanyID && m.Status == "Review")).Count<SegmentArticleComment>();
}

public string GetCompanyDepartmentName(string DepartmentID)
{
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    DataAccessLayer.\u003C\u003Ec__DisplayClass22_0 cDisplayClass220 = new DataAccessLayer.\u003C\u003Ec__DisplayClass22_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass220.DepartmentID = DepartmentID;
    ParameterExpression parameterExpression;
    // ISSUE: method reference
    // ISSUE: field reference
    return this._context.CompanyDepartments.AsNoTracking<CompanyDepartments>().FirstOrDefault<CompanyDepartments>(Expression.Lambda<Func<CompanyDepartments, bool>>((Expression)Expression.Equal((Expression)Expression.Call(n.ID, (MethodInfo)MethodBase.GetMethodFromHandle(__methodref(int.ToString)), Array.Empty<Expression>()), (Expression)Expression.Field((Expression)Expression.Constant((object)cDisplayClass220, typeof(DataAccessLayer.\u003C\u003Ec__DisplayClass22_0)), FieldInfo.GetFieldFromHandle(__fieldref(DataAccessLayer.\u003C\u003Ec__DisplayClass22_0.DepartmentID)))), parameterExpression)).CompanyDepartmentName;
}

public string GetCountryName(string CountryCode)
{
    ParameterExpression parameterExpression;
    // ISSUE: method reference
    // ISSUE: method reference
    this._context.CompanyLocCountries.AsNoTracking<CompanyLocCountries>().OrderBy<CompanyLocCountries, string>((Expression<Func<CompanyLocCountries, string>>)(n => n.country_name)).Where<CompanyLocCountries>((Expression<Func<CompanyLocCountries, bool>>)(n => n.country_code == CountryCode)).Select<CompanyLocCountries, SelectListItem>(Expression.Lambda<Func<CompanyLocCountries, SelectListItem>>((Expression)Expression.MemberInit(Expression.New(typeof(SelectListItem)), (MemberBinding)Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(__methodref(SelectListItem.set_Value)), )))); //unable to render the statement
    this._context.CompanyLocCountries.Find((object)CountryCode);
    this._context.CompanyLocCountries.Where<CompanyLocCountries>((Expression<Func<CompanyLocCountries, bool>>)(m => m.country_code == CountryCode));
    return this._context.CompanyLocCountries.AsNoTracking<CompanyLocCountries>().FirstOrDefault<CompanyLocCountries>((Expression<Func<CompanyLocCountries, bool>>)(m => m.country_code == CountryCode)).country_name;
}

public string GetStateName(string StateCode)
{
    return this._context.CompanyLocStates.AsNoTracking<CompanyLocStates>().FirstOrDefault<CompanyLocStates>((Expression<Func<CompanyLocStates, bool>>)(p => p.state_code == StateCode)).state_name;
}

public void DeleteArticleAndUpdateOthersOrder(
  SegmentArticle article,
  string segmentTitle,
  int orderDeletedArticleIsAt)
{
    DbSet<SegmentArticle> segmentArticle = this._context.SegmentArticle;
    Expression<Func<SegmentArticle, bool>> predicate = (Expression<Func<SegmentArticle, bool>>)(m => m.CompanyID == article.CompanyID && m.Segments.Contains(segmentTitle));
    foreach (SegmentArticle entity in (IEnumerable<SegmentArticle>)segmentArticle.Where<SegmentArticle>(predicate))
    {
        string str1 = "";
        string segments = entity.Segments;
        string[] strArray1 = entity.Segments.Split("|", StringSplitOptions.None);
        foreach (string str2 in strArray1)
        {
            string str3 = "";
            if (((IEnumerable<string>)strArray1).Last<string>() != str2)
                str3 = "|";
            if (str2.Contains(segmentTitle))
            {
                string[] strArray2 = str2.Split("/", StringSplitOptions.None);
                int result;
                int.TryParse(strArray2[1], out result);
                if (result > orderDeletedArticleIsAt)
                    --result;
                str1 = str1 + strArray2[0] + "/" + (object)result + str3;
            }
            else
                str1 = str1 + str2 + str3;
        }
        entity.Segments = str1;
        ((DbContext)this._context).Update<SegmentArticle>(entity);
    }
}

public void DeleteArticleAndUpdateOthersOrder(SegmentArticle article, string segmentTitle)
{
    DbSet<SegmentArticle> segmentArticle1 = this._context.SegmentArticle;
    Expression<Func<SegmentArticle, bool>> predicate = (Expression<Func<SegmentArticle, bool>>)(m => m.CompanyID == article.CompanyID && m.Order > article.Order);
    foreach (SegmentArticle entity in (IEnumerable<SegmentArticle>)segmentArticle1.Where<SegmentArticle>(predicate))
    {
        string str1 = "";
        string segments = entity.Segments;
        string[] strArray1 = entity.Segments.Split("|", StringSplitOptions.None);
        foreach (string str2 in strArray1)
        {
            string str3 = "";
            if (((IEnumerable<string>)strArray1).Last<string>() != str2)
                str3 = "|";
            if (str2.Contains(segmentTitle))
            {
                string[] strArray2 = str2.Split("/", StringSplitOptions.None);
                int result;
                int.TryParse(strArray2[1], out result);
                --result;
                str1 = str1 + strArray2[0] + "/" + (object)result + str3;
            }
            else
                str1 = str1 + str2 + str3;
        }
        entity.Segments = str1;
        SegmentArticle segmentArticle2 = entity;
        int? order = segmentArticle2.Order;
        segmentArticle2.Order = order.HasValue ? new int?(order.GetValueOrDefault() - 1) : new int?();
        ((DbContext)this._context).Update<SegmentArticle>(entity);
    }
    this.RemoveSegmentFromArticleSegments(article, segmentTitle);
    ((DbContext)this._context).SaveChanges();
}

public void RemoveSegmentFromArticleSegments(SegmentArticle art, string segmentTitle)
{
    string str1 = "";
    string segments = art.Segments;
    foreach (string str2 in art.Segments.Split("|", StringSplitOptions.None))
    {
        if (!str2.Contains(segmentTitle))
            str1 += str2;
    }
    art.Segments = str1;
    ((DbContext)this._context).Update<SegmentArticle>(art);
    ((DbContext)this._context).SaveChanges();
}
  }
}
