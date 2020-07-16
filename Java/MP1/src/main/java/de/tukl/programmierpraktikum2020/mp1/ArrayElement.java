package de.tukl.programmierpraktikum2020.mp1;

public class ArrayElement<String, Integer> {
    private String key;
    private Integer value;
    public ArrayElement(String key, Integer value) {
        this.key = key;
        this.value = value;
    }

    public String getKey() {
        return key;
    }

    public void setKey(String key) {
        this.key = key;
    }

    public Integer getValue() {
        return value;
    }

    public void setValue(Integer value) {
        this.value = value;
    }
}
