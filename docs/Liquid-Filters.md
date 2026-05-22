---
title: Liquid filters
---

# Liquid filters (composite fields)

Composite fields (`"$type": "composite"`) use [Liquid](https://shopify.github.io/liquid/) templates to assemble a value from other fields. All the standard Liquid filters are available, plus a few custom filters described below.

> Field names are normalised: a dash (`-`) in a field name becomes an underscore (`_`) inside the template. Both forms can be used interchangeably.

{% raw %}
## `format`

Formats a value using a standard .NET format string.

**Syntax:** `{{ value | format:"format_string" }}`

```liquid
{{packet_number | format:"D4"}}      → 0042
{{amount | format:"F2"}}             → 123.45
{{date   | format:"yyyy-MM-dd"}}     → 2024-01-15
{{ratio  | format:"P2"}}             → 75.50%
```

Common format strings: `D4`/`D6` (zero-padded decimals), `F2` (fixed point), `N0` (thousands separator), `C` (currency), `P` (percentage).

## `pad_left`

Left-pads a value to a given length (default character: `0`).

```liquid
{{packet_number | pad_left:4}}        → 0042
{{code          | pad_left:5,'X'}}    → XXX42
```

## `pad_right`

Right-pads a value to a given length (default character: space).

```liquid
{{name | pad_right:20}}               → "John                "
{{code | pad_right:10,'0'}}           → "ABC0000000"
```

## Chaining

All filters can be chained:

```liquid
{{value  | upcase | pad_right:20}}
{{number | format:"F2" | prepend:"$"}}
```
{% endraw %}

> If you need an additional filter that is not provided out of the box, see the [Plugin Development]({{ '/Plugin-Development' | relative_url }}) page — custom filters can be added to OmniGenerator itself.

See the [Liquid for designers](https://shopify.github.io/liquid/) reference for the complete list of standard filters.
