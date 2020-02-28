# Почему мы лохи?

## Потому, что надо знать

- GitHub
- UnitTest
- CI-build

## Про тесты

Существует множество различных видов тестирования (классификация по размеру тестирования), но их всех называют Unit Test. Нужны для проверки работоспособности программы в определенных условиях, **но не для доказательства корректности программы**.

### Как писать Unit Test?

- Задать крайние, так сказать, патологические случаи.
- Добавить общие случаи (то, что будет в среднем).
- Тесты должны иметь такой размер, чтобы их можно было проверить вручную.
- По падению теста надо иметь возможность локализовать проблему.
- Unit Test -- это еще и документация, т. к. они, помимо прочего, показывают, как вызывать функции.

### Примеры систем тестирования:

- MS Test
- NUnit

Комментарии должны использоваться аккуратно: только тогда, когда идея, стоящая за кодом, нетривиальна даже для могущего код читать.

## Про CI-build

Иногда бывает, что мы не можем запустить проект, написанный на одном компьютере, на другом компьютере. Для решения этой проблемы существуют CI-сервера -- машины, которые пытаются запустить проект в различных конфигурациях и сообщают, если проект не запустился или не прошли тесты.

Особенно это удобно, т. к. мы можем автоматизировать проверки и запускать их при каждом push, например.

А еще можно проверять следование соглашениям о стиле кода, а еще можно делать статический анализ кода (не всегда) и т. д.

[Узнать больше](./about_CI.pdf)

### Пример

- GitHub CI-build Travice

## Как сдавать задачи?

Есть репозиорий. Задачи решаем в отдельных ветках. Когда мы думаем, что решили задачу, надо ее проверить CI-build-ами. Если работает сделать pull request к себе, добавив в кач. редактора преподавателя (имя на GitHub-е -- gsvgit).

Зачет ставится по задачам (решить все). За нарушение дедлайнов будут штрафные задачи.

**Да, это жестко.**

## Итоги

Нас за пршедшее время так и не научили программировать. Но, надеюсь, вскоре все изменится.