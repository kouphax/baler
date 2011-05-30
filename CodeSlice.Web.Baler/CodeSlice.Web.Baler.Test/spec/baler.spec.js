describe("Bale Single File", function(){
   
    beforeEach(function(){
        this.addMatchers({
            toBeHidden: function(){
                return window.getComputedStyle(this.actual, null).getPropertyValue('display') == 'none';
            }
        });
    });

    it("should successfuly bale a single javascript file without changing the behaviour", function(){
        expect(window.globalSimple).toBeDefined();
        expect(window.globalSimple).toBe("success");
    });

    it("should successfuly bale a single stylesheet without changing the behaviour", function(){
        expect(document.getElementById('single')).toBeHidden();
    });
});