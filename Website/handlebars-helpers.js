module.exports = {
    ifTripleAnd: function(v1, v2, v3, options) {
        if(v1 && v2 && v3)
            return options.fn(this);
        return options.inverse(this);
    },
    ifNull: function(v1, options) {
        if(v1 === null)
            return options.fn(this);
        return options.inverse(this);
    },
    ifNotNull: function(v1, options) {
        if(v1 != null)
            return options.fn(this);
        return options.inverse(this);
    }
}
