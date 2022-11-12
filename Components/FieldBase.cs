using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace IdentityServerConfig.Components
{
    public abstract class FieldBase<TEntity, TProp> : ComponentBase
    {
        private Expression<Func<TEntity, TProp>>? _lastAssignedProperty;
        private string? _propertyName;
        private TProp? _propertyValue;

        [Parameter, EditorRequired]
        public TEntity Entity { get; set; } = default!;

        [Parameter, EditorRequired] public Expression<Func<TEntity, TProp>> Property { get; set; } = default!;

        [Parameter, EditorRequired]
        public string DisplayName { get; set; } = default!;

        protected string? PropertyName { get => _propertyName; }

        protected TProp? PropertyValue { get => _propertyValue; }

        protected override void OnParametersSet()
        {
            if (_lastAssignedProperty != Property)
            {
                _lastAssignedProperty = Property;
                var compiledPropertyExpression = Property.Compile();

                if (Property.Body is MemberExpression memberExpression)
                {
                    _propertyName = memberExpression.Member.Name;
                }

                _propertyValue = compiledPropertyExpression.Invoke(Entity);
            }
        }
    }
}
