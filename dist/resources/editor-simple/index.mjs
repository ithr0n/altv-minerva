import alt from 'alt-server';

alt.onClient('serverEvalExecute', (player, evalCode) => {
    eval(evalCode);
});
