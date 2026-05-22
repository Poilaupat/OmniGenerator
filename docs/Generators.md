---
title: Field generators
---

# Field generators

A **field generator** produces the value of a single field for a single element instance. Each generator is described by a JSON object that lives inside the `fields` array of an element (root, group or document).

Every field shares two common properties:

| Property | Description |
|----------|-------------|
| `$type` | Type of generator. One of: `constant`, `regex`, `numeric`, `date`, `increment`, `list`, `composite`, `key`, `aggregate`. |
| `name` | Field name. Must be unique inside its parent element. |

The other properties depend on the generator type and are described below.

---

## `constant`

Always returns the same value.

```json
{ "$type": "constant", "name": "code-org", "value": "30002" }
```

| Property | Description |
|----------|-------------|
| `value` | The constant string returned every time. |

---

## `numeric`

Generates a random number in `[min, max]`.

```json
{ "$type": "numeric", "name": "amount", "min": 100, "max": 99999 }
```

| Property | Description |
|----------|-------------|
| `min` | Lower bound (default `0.01`). |
| `max` | Upper bound (default `10 000 000`). |

---

## `regex`

Generates a value matching a regular expression.

```json
{ "$type": "regex", "name": "dataread", "pattern": "\\d{7} \\d{9}908 \\d{12}" }
```

| Property | Description |
|----------|-------------|
| `pattern` | A regular expression. OmniGenerator produces a random value that matches this pattern. |

---

## `date`

Generates a random date within a window expressed in days relative to today.

```json
{ "$type": "date", "name": "date", "day-diff-min": -400, "day-diff-max": -3 }
```

| Property | Description |
|----------|-------------|
| `day-diff-min` | Minimum day offset from today (negative for past). |
| `day-diff-max` | Maximum day offset from today (negative for past). |

---

## `increment`

Returns a monotonically increasing integer.

```json
{ "$type": "increment", "name": "seq", "start": 42, "increment": 1 }
```

| Property | Description |
|----------|-------------|
| `start` | Starting value (default `0`). |
| `increment` | Step added at each call (default `1`). |

---

## `list`

Picks a value from a weighted list. The list can be inlined or loaded from an external file.

**Inline (equal weights):**

```json
{ "$type": "list", "name": "dice", "list": ["one","two","three","four","five","six"] }
```

**Inline (custom weights):**

```json
{
  "$type": "list",
  "name": "two-dices",
  "list": { "two": 1, "three": 2, "four": 3, "five": 4, "six": 5, "seven": 6 }
}
```

**External file:**

```json
{
  "$type": "list",
  "name": "bank-zip-city",
  "list-file": "./Resources/Lists/zipcodes_cities.txt"
}
```

| Property | Description |
|----------|-------------|
| `list` | Inline values, either as an array (equal weights) or as an object `value → weight`. |
| `list-file` | Path to an external list file. Mutually exclusive with `list`. |

---

## `composite`

Builds a value from other fields of the same element using a [Liquid](https://shopify.github.io/liquid/) template. See [Liquid Filters]({{ '/Liquid-Filters' | relative_url }}) for the custom filters available in addition to the standard ones (`format`, `pad_left`, `pad_right`, …).

{% raw %}
```json
{
  "$type": "composite",
  "name": "payor-name",
  "dependent-upon": "payor-first-name,payor-last-name",
  "format": "{{payor-first-name}} {{payor-last-name}}"
}
```
{% endraw %}

| Property | Description |
|----------|-------------|
| `format` | Liquid template. Field names containing `-` are normalised to `_` internally; you may use either form in the template. |
| `dependent-upon` | Comma-separated list of field names this template depends on. Generation is ordered so those fields are produced first. |

---

## `key`

Computes a checksum / control key from another field's value.

```json
{
  "$type": "key",
  "name": "rlmc",
  "dependent-upon": "dataread",
  "key-type": "rlmc"
}
```

| Property | Description |
|----------|-------------|
| `dependent-upon` | The single field whose value feeds the algorithm. |
| `key-type` | Algorithm name. One of: `dummy`, `rlmc`, `rib`, `tip`, `tipGroup6`, `ics`, `iban`. |

| `key-type` | Meaning |
|------------|---------|
| `dummy` | Test/dummy key — mostly used in unit tests. |
| `rlmc` | RLMC (Recomposition Ligne Magnétique Chèque). |
| `rib` | RIB (Relevé d'Identité Bancaire). |
| `tip` | TIP (Titre Interbancaire de Paiement). |
| `tipGroup6` | TIP key for the group-6 segment. |
| `ics` | ICS (Identifiant Créancier SEPA). |
| `iban` | IBAN check digits. |

---

## `aggregate`

Aggregates a field over the children of the current element. Typical use: total amount and document count on a remittance.

```json
{
  "$type": "aggregate",
  "name": "total",
  "aggregate-type": "Sum",
  "scope": "AllChildren",
  "target-element": "cheque",
  "dependent-upon": "amount"
}
```

| Property | Description |
|----------|-------------|
| `aggregate-type` | `Sum` (sums numeric values) or `Count` (counts matching children). |
| `scope` | `DirectChildren` (children of the current element) or `AllChildren` (descendants at any depth). |
| `target-element` | Name of the child element to aggregate over. |
| `dependent-upon` | For `Sum`: the name of the field whose values are summed. For `Count` it can be left empty. |

Aggregate fields are evaluated after their children have been generated.

---

## Execution order

OmniGenerator computes the order in which fields are generated based on their dependencies (`dependent-upon` for `composite`, `key` and `aggregate`). Fields with no dependency are generated first, then the fields that depend on them, and so on. Circular dependencies are not allowed and will result in an error at load time.
