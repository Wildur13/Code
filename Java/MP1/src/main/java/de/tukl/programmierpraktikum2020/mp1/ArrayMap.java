package de.tukl.programmierpraktikum2020.mp1;

@SuppressWarnings("unchecked")
public class ArrayMap<String, Integer> implements Map<String, Integer> {

    ArrayElement<String, Integer>[] arrayElements;
    public ArrayMap() {
        this.arrayElements = new ArrayElement[0];
    }

    @Override
    public Integer get(String key) {
        for (ArrayElement<String, Integer> arrayElement : arrayElements) {
            if (arrayElement.getKey() == key) {
                return arrayElement.getValue();
            }
        }
        return null;
    }

    @Override
    public void put(String key, Integer value) {
        ArrayElement<String, Integer> newArrayElement = new ArrayElement<>(key, value);
        if (arrayElements.length == 0) {
            arrayElements = new ArrayElement[1];
            arrayElements[0] = newArrayElement;
        } else {
            ArrayElement<String, Integer>[] newArrayElements = new ArrayElement[arrayElements.length + 1];
            for (int i = 0; i < arrayElements.length; i++) {
                if (arrayElements[i].getKey() == key) {
                    arrayElements[i].setValue(value);
                    return;
                }
                newArrayElements[i] = arrayElements[i];
            }
            newArrayElements[arrayElements.length] = newArrayElement;
            arrayElements = newArrayElements;
        }
    }

    @Override
    public void remove(String key) {
        boolean isContained = false;
        for (ArrayElement<String, Integer> arrayElement : arrayElements) {
            if (arrayElement.getKey() == key) {
                isContained = true;
                break;
            }
        }
        if (isContained) {
            ArrayElement<String, Integer>[] newArrayElements = new ArrayElement[arrayElements.length - 1];
            for (int i = 0; i < arrayElements.length; i++) {
                if (arrayElements[i].getKey() == key) {
                    ArrayElement<String, Integer> tmp = arrayElements[i];
                    arrayElements[i] = arrayElements[arrayElements.length - 1];
                    arrayElements[arrayElements.length - 1] = tmp;
                }
                if (i < arrayElements.length - 1) {
                    newArrayElements[i] = arrayElements[i];
                }
            }
            arrayElements = newArrayElements;
        }
    }

    @Override
    public int size() {
        return this.arrayElements.length;
    }

    @Override
    public void keys(String[] array) {
        if (array == null || array.length < arrayElements.length) {
            throw new IllegalArgumentException();
        } else {
            for (ArrayElement<String, Integer> arrayElement : arrayElements) {
                insert(arrayElement, array);
            }
        }
    }

    private void insert(ArrayElement<String, Integer> arrayElement, String[] array) {
        for (int i = 0; i < array.length; i++) {
            if (array[i] == null) {
                array[i] = arrayElement.getKey();
                return;
            }
        }
    }
}
