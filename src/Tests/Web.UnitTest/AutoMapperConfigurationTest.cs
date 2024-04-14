using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FluentAssertions;
using Models.Attributes;
using Web.UnitTest.Helpers;

namespace Web.UnitTest
{
    public class AutoMapperConfigurationTest
    {
        private static readonly HashSet<string> AutoMapperTestIgnoredTypes = new()
        {
            // "Web.TenantName",
        };

        private static readonly HashSet<string> AutoMapperTestIgnoredAttributes = new()
        {
            // "TestAttribute",
        };

        public static IEnumerable<object[]> TestAutoMapperConfigurationTestCases
        {
            get
            {
                return MockServiceBuilder.GetMapperConfiguration()
                    .GetAllTypeMaps()
                    .Where(t => !IsMappingIgnored(t.SourceType.FullName!, t.DestinationType.FullName!))
                    .Select(t => new object[] { t.SourceType.FullName!, t.DestinationType.FullName! });
            }
        }

        [Theory]
        [MemberData(nameof(TestAutoMapperConfigurationTestCases))]
        public void TestAutoMapperConfiguration(string source, string destination)
        {
            var config = MockServiceBuilder.GetMapperConfiguration();
            config.AssertConfigurationIsValid();

            var typeMap = config.GetAllTypeMaps().FirstOrDefault(m => m.SourceType.FullName!.Equals(source, StringComparison.OrdinalIgnoreCase) && m.DestinationType.FullName!.Equals(destination, StringComparison.OrdinalIgnoreCase));
            if (typeMap == null)
                throw new Exception("Type map not found");

            foreach (var propertyMap in typeMap.PropertyMaps)
            {
                // please check all custom mappings defined in the profile
                if (propertyMap.CustomMapExpression != null)
                    continue;

                if (propertyMap.SourceMember == null || propertyMap.DestinationMember == null)
                    continue;

                if (IsMappingIgnored($"{typeMap.SourceType.FullName}.{propertyMap.SourceMember.Name}", $"{typeMap.DestinationType.FullName}.{propertyMap.DestinationMember.Name}"))
                    continue;

                var src = propertyMap.SourceMember.CustomAttributes.Where(FilterAttribute).Select(ConvertAttribute).ToArray();
                var dst = propertyMap.DestinationMember.CustomAttributes.Where(FilterAttribute).Select(ConvertAttribute).ToArray();
                src.Should().BeEquivalentTo(dst, c => c.WithTracing(), $"for {typeMap.SourceType.FullName}.{propertyMap.SourceMember.Name} -> {typeMap.DestinationType.FullName}.{propertyMap.DestinationMember.Name}");
            }
        }

        private static bool FilterAttribute(CustomAttributeData a)
        {
            if (AutoMapperTestIgnoredAttributes.Contains(a.AttributeType.FullName!))
                return false;

            if (StringComparer.OrdinalIgnoreCase.Equals(a.AttributeType.Namespace, typeof(MaxLengthAttribute).Namespace))
                return true;

            return false;
        }

        private static string ConvertAttribute(CustomAttributeData ca)
        {
            return ca.AttributeType.Name switch
            {
                nameof(NotRequiredByApiAttribute) => $"{typeof(RequiredAttribute).FullName}, {string.Join(",", ca.ConstructorArguments.Select(arg => arg.ToString()))}",
                _ => $"{ca.AttributeType.FullName}, {string.Join(",", ca.ConstructorArguments.Select(arg => arg.ToString()))}"
            };
        }

        private static bool IsMappingIgnored(string sourcePath, string targetPath)
        {
            return AutoMapperTestIgnoredTypes.Any(c => sourcePath.StartsWith(c, StringComparison.Ordinal) || targetPath.StartsWith(c, StringComparison.Ordinal));
        }
    }
}
