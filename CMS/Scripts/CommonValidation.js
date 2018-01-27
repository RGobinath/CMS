function NumbersOnly(value) {
    if (isNaN(value)) {
        ErrMsg("Numbers only allowed.");
        return false;
    }
}