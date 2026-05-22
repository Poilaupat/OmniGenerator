---
applyTo: "**/*.Test*/**,**/*Tests.cs,**/*Test.cs"
---

# Instructions pour les tests

- Framework : NUnit
- Assertions : NUnit (Assert.That uniquement, pas FluentAssertions ici)


# Instructions spécifiques aux Packagers et Renderers de OmniGenerator

- Tester les méthodes de FieldExtractorBase une fois pour toutes dans un fichier de test à part
- Dans les classes enfant de FieldExtractorBase, ne surtout pas retester tous les cas et se borner à :
    - Tester que les accesseurs retournent la valeur du bon champ cible depuis la FieldCollection
    - Tester les retours le valeurs par défaut dans le cas d'accesseurs optionnels
    - Vérifier l'adéquation entre la décoration FieldInfo -> FieldName et le field cible
    - Vérifier l'adéquation entre la décoration FieldInfo -> IsRequired et le type d'accesseur (required vs optionnal with defaut value)
