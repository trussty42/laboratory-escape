# Laboratory-Escape
## Концепция игры
- Игрок движется по клеткам (усложнение клетки могут быть разных типов: замедляющие, отнимающие здоровье, лечащие и т.п.).
- Враги имеют диапазон зрения (это просто треугольник от их позиции), у каждого типа врагов будут разные возможности: дальность видимости, дальность поражения (возможно здоровье).
- Если игрок в конусе или движется в нем — его «заметность» растёт в зависимости от того насколько он близко к врагу.
- При 100% заметности — поражение.
- Существуют некоторые объекты усиления подбирая которые игрок будет получать дополнительное здоровье (или расти).
- Цель игрока добраться до выхода и остаться непойманным

## Основные элементы игры

1. **Игровое поле**:
   - Сетка из клеток разных типов
   - Препятствия и укрытия
   - Выход (цель уровня)

2. **Типы клеток**:
   - Обычные (нейтральные)
   - Замедляющие
   - Повреждающие
   - Лечащие

3. **Персонажи**:
   - Игрок
   - Охранники разных типов

4. **Механика обнаружения**:
   - Конусы зрения у охранников
   - Уровень заметности игрока

5. **Предметы**:
   - Предмет улучшение скорости игрока
  
## План

---

### **Неделя 1: Базовые сущности и движение**
**Задачи:**
1. Создать базовые классы
2. Настроить движение по сетке

---

### **Неделя 2: Система обнаружения и поведения**
**Задачи:**
1. Доработать охранников
2. Система обнаружения игрока
3. Простые поведенческие алгоритмы:
   - Патрульные маршруты
   - Реакция на шум

---

### **Неделя 3: Способности и прогресс**
**Задачи:**
1. Добавить способности игрока
2. Система репутации/тревоги:
   - Уровень тревоги влияет на скорость охранников
   - Шкала заполняется при обнаружении
3. Механика побега:
   - Несколько выходов с разными условиями
   - Временные укрытия

---

### **Неделя 4: Создание уровней**
**Задачи:**
1. Создать 5 уровней с нарастающей сложностью:
   - Лаборатория → Коридоры → Хранилище → и т.д.

---

### **Неделя 5: Тестирование**
**Задачи:**
1. Тестирование и доработка недостатков

---
