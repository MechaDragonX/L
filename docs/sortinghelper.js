const compareTo = (prop, item1, item2) => {
    switch(prop) {
        case 'surname':
            if(item2.surname > item1.surname)
                return 1;
            if(item2.surname < item1.surname)
                return -1;
            return 0;
        case 'givenName':
            if(item2.givenName > item1.givenName)
                return 1;
            if(item2.givenName < item1.givenName)
                return -1;
            return 0;
        case 'email':
            if(item2.email > item1.email)
                return 1;
            if(item2.email < item1.email)
                return -1;
            return 0;
        case 'highSchool':
            if(item2.highSchool > item1.highSchool)
                return 1;
            if(item2.highSchool < item1.highSchool)
                return -1;
            return 0;
        case 'collegeUG':
            if(item2.collegeUG > item1.collegeUG)
                return 1;
            if(item2.collegeUG < item1.collegeUG)
                return -1;
            return 0;
        case 'collegePG':
            if(item2.collegePG > item1.collegePG)
            return 1;
            if(item2.collegePG < item1.collegePG)
                return -1;
            return 0;
    }
};

const mergeSort = (array, prop)  => {
    if (array.length >= 2) {
        let left  = array.slice(0, array.length / 2)
        console.log('left: ' + left);
        let right = array.slice(array.length/2, array.length);
        console.log('right: ' + right);

        mergeSort(left);
        console.log('sorted left: ' + left);
        mergeSort(right);
        console.log('sorted right: ' + right);
		merge(array, left, right, prop);
    }
}
// prop refers to the property of the applicant that is being sorted
const merge = (result, left, right, prop) => {
    let i1 = 0;
    let i2 = 0;

    for(i = 0; i < result.length; i++) {
        if(i2 >= right.length || 
            (i1 < left.length && (compareTo(prop, left[i1], right[i2]) == 1 || compareTo(prop, left[i1], right[i2]) == 0))) {
            result[i] = left[i1];
            i1++;
        } else {
            result[i] = right[i2];
            i2++;
        }
    }
};

const sort = (applicants, prop) => {
    switch(prop) {
        case 'id':
            console.log('I was fired!');
            return sortId(applicants);
        case 'surname':
            console.log('I was fired!');
            return sortSurname(applicants);
        case 'givenName':
            console.log('I was fired!');
            return sortGivenName(applicants);
        case 'email':
            console.log('I was fired!');
            let sorted = sortEmail(applicants);
            console.log(sorted);
            return sorted;
        default:
            console.log('blarg!');
            return applicants;
    }
};

const sortId = (applicants) => {
    mergeSort(applicants, 'id');
};
const sortSurname = (applicants) => {
    mergeSort(applicants, 'surname');
};
const sortGivenName = (applicants) => {
    mergeSort(applicants, 'givenName');
};
const sortEmail = (applicants) => {
    mergeSort(applicants, 'email');
};

module.exports = {
    mergeSort,
    merge,
    sort,
    sortId,
    sortSurname,
    sortGivenName,
    sortEmail
};
